using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using static ShireBudgeters.Common.DTOs.AuthDTOs;
using static ShireBudgeters.Common.Common.Helpers.IdentityHelpers;
using ShireBudgeters.DA.Models;
using ShireBudgeters.BL.Common.Mappings;

namespace ShireBudgeters.BL.Services.Identity;

internal class IdentityService(
    ILogger<IdentityService> logger,
    SignInManager<UserModel> signInManager,
    UserManager<UserModel> userManager) : IIdentityService
{
    private readonly ILogger<IdentityService> _logger = logger;
    private readonly UserManager<UserModel> _userManager = userManager;

    public async Task<UserInfoDTO> GetCurrentUserAsync()
    {
        if (signInManager.Context.User.Identity?.IsAuthenticated ?? false)
        {
            var user = await _userManager.GetUserAsync(signInManager.Context.User);

            if (user is not null && user.IsActive)
            {
                return user.ToUserInfoDTO();
            }
        }
        return default!;
    }

    public async Task<LoginResponseDTO> LoginAsync(LoginRequestDTO request)
    {
        var maskedEmail = MaskEmail(request?.Email);
        var timestamp = DateTimeOffset.UtcNow;

        if (request is null || request.Email is null || request.Password is null)
        {
            _logger.LogWarning(
                "Security Event: Invalid login request - Missing email or password. Timestamp: {Timestamp}",
                timestamp);
            return new LoginResponseDTO(null, false, "Invalid email or password");
        }

        var user = await _userManager.FindByEmailAsync(request.Email);

        #region User Validation

        // Check if user exists
        if (user is null)
        {
            _logger.LogWarning(
                "Security Event: Failed login attempt - User not found. Email: {MaskedEmail}, Timestamp: {Timestamp}",
                maskedEmail, timestamp);
            return new LoginResponseDTO(null, false, "Invalid email or password");
        }

        // Check if account is active
        if (user.IsActive is false)
        {
            _logger.LogWarning(
                "Security Event: Failed login attempt - Inactive account. Email: {MaskedEmail}, UserId: {UserId}, Timestamp: {Timestamp}",
                maskedEmail, user.Id, timestamp);
            return new LoginResponseDTO(null, false, "Invalid email or password");
        }

        // Check if account is locked
        if (await _userManager.IsLockedOutAsync(user))
        {
            var lockoutEnd = await _userManager.GetLockoutEndDateAsync(user);
            _logger.LogError(
                "Security Event: Account lockout - Login attempt on locked account. Email: {MaskedEmail}, UserId: {UserId}, LockoutEnd: {LockoutEnd}, Timestamp: {Timestamp}",
                maskedEmail, user.Id, lockoutEnd, timestamp);
            return new LoginResponseDTO(null, false, "Your account is locked, please try again later");
        }

        // Check if password is correct
        if ((await _userManager.CheckPasswordAsync(user, request.Password)) is false)
        {
            var accessFailedCount = await _userManager.GetAccessFailedCountAsync(user);
            _logger.LogWarning(
                "Security Event: Failed login attempt - Invalid password. Email: {MaskedEmail}, UserId: {UserId}, FailedAttempts: {FailedAttempts}, Timestamp: {Timestamp}",
                maskedEmail, user.Id, accessFailedCount, timestamp);
            return new LoginResponseDTO(null, false, "Invalid email or password");
        }

        // Check if account is locked (early return with clear message)
        if (await _userManager.IsLockedOutAsync(user))
        {
            var lockoutEnd = await _userManager.GetLockoutEndDateAsync(user);
            var message = lockoutEnd.HasValue && lockoutEnd.Value > DateTimeOffset.UtcNow
                ? $"Your account is locked until {lockoutEnd.Value:g}. Please try again later."
                : "Your account is locked. Please try again later.";

            _logger.LogError(
                "Security Event: Account lockout - Login attempt on locked account (post-validation). Email: {MaskedEmail}, UserId: {UserId}, LockoutEnd: {LockoutEnd}, Timestamp: {Timestamp}",
                maskedEmail, user.Id, lockoutEnd, timestamp);

            return new LoginResponseDTO(null, false, message);
        }

        #endregion

        // PasswordSignInAsync will handle password validation, failed attempts, and lockout
        var result = await signInManager.PasswordSignInAsync(
            request.Email,
            request.Password,
            isPersistent: request.RememberMe,
            lockoutOnFailure: false);

        if (result.Succeeded is false)
        {
            // Check if account was just locked
            if (result.IsLockedOut)
            {
                var lockoutEnd = await _userManager.GetLockoutEndDateAsync(user);
                _logger.LogError(
                    "Security Event: Account lockout - Account locked after failed login attempt. Email: {MaskedEmail}, UserId: {UserId}, LockoutEnd: {LockoutEnd}, Timestamp: {Timestamp}",
                    maskedEmail, user.Id, lockoutEnd, timestamp);
                return new LoginResponseDTO(null, false, "Your account has been locked due to multiple failed login attempts. Please try again later.");
            }

            _logger.LogWarning(
                "Security Event: Failed login attempt - Sign-in failed. Email: {MaskedEmail}, UserId: {UserId}, Result: {Result}, Timestamp: {Timestamp}",
                maskedEmail, user.Id, result.ToString(), timestamp);
            return new LoginResponseDTO(null, false, "Invalid email or password");
        }

        // Reset access failed count on successful login
        await _userManager.ResetAccessFailedCountAsync(user);

        // Update last login date
        user.LastLoginDate = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        // Log successful login
        _logger.LogInformation(
            "Security Event: Successful login. Email: {MaskedEmail}, UserId: {UserId}, RememberMe: {RememberMe}, Timestamp: {Timestamp}",
            maskedEmail, user.Id, request.RememberMe, timestamp);

        return new LoginResponseDTO(user.ToUserInfoDTO());
    }

    public async Task LogoutAsync()
    {
        var timestamp = DateTimeOffset.UtcNow;

        // Try to get the current user before logout
        UserModel? user = null;
        string maskedEmail = "[UNKNOWN]";

        if (signInManager.Context.User.Identity?.IsAuthenticated ?? false)
        {
            user = await _userManager.GetUserAsync(signInManager.Context.User);
            if (user is not null)
            {
                maskedEmail = MaskEmail(user.Email);
            }
        }

        await signInManager.SignOutAsync();

        if (user is not null)
        {
            _logger.LogInformation(
                "Security Event: User logout. Email: {MaskedEmail}, UserId: {UserId}, Timestamp: {Timestamp}",
                maskedEmail, user.Id, timestamp);
        }
        else
        {
            _logger.LogInformation(
                "Security Event: Logout attempt - No authenticated user found. Timestamp: {Timestamp}",
                timestamp);
        }
    }

    /// <summary>
    /// Create a new user with the specified password. Used only for initial seeding.
    /// </summary>
    public async Task<string> CreateUserWithPasswordAsync(
        string email,
        string password,
        string firstName,
        string lastName)
    {
        var maskedEmail = MaskEmail(email);
        var timestamp = DateTimeOffset.UtcNow;

        var user = new UserModel
        {
            UserName = email,
            Email = email,
            NormalizedUserName = email.ToUpperInvariant(),
            NormalizedEmail = email.ToUpperInvariant(),
            FirstName = firstName,
            LastName = lastName,
            IsActive = true,
            EmailConfirmed = false,
            CreatedBy = "system",
            CreatedDate = DateTime.UtcNow,
            ModifiedBy = "system",
            ModifiedDate = DateTime.UtcNow
        };

        // This automatically hashes the password and sets SecurityStamp, ConcurrencyStamp, etc.
        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            _logger.LogError(
                "Security Event: User creation failed. Email: {MaskedEmail}, Errors: {Errors}, Timestamp: {Timestamp}",
                maskedEmail, errors, timestamp);
            throw new InvalidOperationException($"Failed to create user: {errors}");
        }

        _logger.LogInformation(
            "Security Event: User created successfully. Email: {MaskedEmail}, UserId: {UserId}, Timestamp: {Timestamp}",
            maskedEmail, user.Id, timestamp);

        return user.Id;
    }
}
