namespace ShireBudgeters.Common.DTOs;

/// <summary>
/// Data Transfer Objects for authentication operations.
/// </summary>
public class AuthDTOs
{
    /// <summary>
    /// Request DTO for user login.
    /// </summary>
    public record LoginRequestDTO(string Email, string Password, bool RememberMe);
    
    /// <summary>
    /// Response DTO for user login containing success status, error message, and user information.
    /// </summary>
    public record LoginResponseDTO(UserInfoDTO? User, bool Success = true, string? ErrorMessage = null);

    /// <summary>
    /// DTO containing user information for API responses.
    /// </summary>
    public record UserInfoDTO(string Id, string Email, string FirstName, string LastName);
}
