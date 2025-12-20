using static ShireBudgeters.Common.DTOs.AuthDTOs;

namespace ShireBudgeters.BL.Services.Identity;

public interface IIdentityService
{
    Task<LoginResponseDTO> LoginAsync(LoginRequestDTO request);
    Task LogoutAsync();
    Task<UserInfoDTO> GetCurrentUserAsync();
    Task<string> CreateUserWithPasswordAsync(string email, string password, string firstName, string lastName);
}
