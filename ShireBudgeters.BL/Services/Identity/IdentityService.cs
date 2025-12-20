using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ShireBudgeters.BL.Common.Mappings;
using static ShireBudgeters.Common.DTOs.AuthDTOs;
using ShireBudgeters.DA.Models;

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
        if (request is null || request.Email is null || request.Password is null)
        {
            return new LoginResponseDTO(null, false, "Invalid email or password");
        }

        var user = await _userManager.FindByEmailAsync(request.Email);

        #region User Validation

        // Check if user exists
        if (user is null)
        {
            _logger.LogWarning("User not found for email: {Email}", request.Email);
            return new LoginResponseDTO(null, false, "Invalid email or password");
        }

        // Check if account is active
        if (user.IsActive is false)
        {
            _logger.LogWarning("Login attempt for inactive account: {Email}", request.Email);
            return new LoginResponseDTO(null, false, "Invalid email or password");
        }

        // Check if account is locked
        if (await _userManager.IsLockedOutAsync(user))
        {
            return new LoginResponseDTO(null, false, "Your account is locked, please try again later");
        }

        // Check if password is correct
        if ((await _userManager.CheckPasswordAsync(user, request.Password)) is false)
        {
            _logger.LogWarning("Invalid password attempt for email: {Email}", request.Email);
            return new LoginResponseDTO(null, false, "Invalid email or password");
        }

        // Check if account is locked (early return with clear message)
        if (await _userManager.IsLockedOutAsync(user))
        {
            var lockoutEnd = await _userManager.GetLockoutEndDateAsync(user);
            var message = lockoutEnd.HasValue && lockoutEnd.Value > DateTimeOffset.UtcNow
                ? $"Your account is locked until {lockoutEnd.Value:g}. Please try again later."
                : "Your account is locked. Please try again later.";
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
                _logger.LogWarning("Account locked after failed login attempt: {Email}", request.Email);
                return new LoginResponseDTO(null, false, "Your account has been locked due to multiple failed login attempts. Please try again later.");
            }

            _logger.LogWarning("Login failed for email: {Email}, Result: {Result}", request.Email, result);
            return new LoginResponseDTO(null, false, "Invalid email or password");
        }

        // Reset access failed count on successful login
        await _userManager.AccessFailedAsync(user);

        // Update last login date
        user.LastLoginDate = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        return new LoginResponseDTO(user.ToUserInfoDTO());
    }

    public async Task LogoutAsync() => await signInManager.SignOutAsync();
}
