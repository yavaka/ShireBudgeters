using static ShireBudgeters.Common.DTOs.AuthDTOs;

namespace ShireBudgeters.BL.Services.Identity;

public interface IIdentityService
{
    LoginRequestDTO LoginAsync(LoginRequestDTO request);
    void LogoutAsync();
    UserInfoDTO GetCurrentUserAsync();
}
