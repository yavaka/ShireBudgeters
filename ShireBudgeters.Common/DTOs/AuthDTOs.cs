namespace ShireBudgeters.Common.DTOs;

/// <summary>
/// Data Transfer Objects for authentication operations.
/// </summary>
public class AuthDTOs
{
    /// <summary>
    /// Request DTO for user login.
    /// </summary>
    public class LoginRequestDTO
    {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public bool RememberMe { get; set; }
    };
    
    /// <summary>
    /// Response DTO for user login containing success status, error message, and user information.
    /// </summary>
    public record LoginResponseDTO(UserInfoDTO? User, bool Success = true, string? ErrorMessage = null);

    /// <summary>
    /// Request DTO for user registration.
    /// </summary>
    public class RegisterRequestDTO
    {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string ConfirmPassword { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
    }

    /// <summary>
    /// Response DTO for user registration containing success status, error message, and user information.
    /// </summary>
    public record RegisterResponseDTO(UserInfoDTO? User, bool Success = true, string? ErrorMessage = null);

    /// <summary>
    /// DTO containing user information for API responses.
    /// </summary>
    public record UserInfoDTO(string Id, string Email, string FirstName, string LastName);
}
