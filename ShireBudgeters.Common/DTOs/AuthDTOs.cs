namespace ShireBudgeters.Common.DTOs
{
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
        public record LoginResponseDTO(bool Success, string? ErrorMessage, UserInfoDTO? User);

        /// <summary>
        /// DTO containing user information for API responses.
        /// </summary>
        public record UserInfoDTO(string Id, string Email, string FirstName, string LastName, IList<string> Roles);
    }
}
