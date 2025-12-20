using Microsoft.AspNetCore.Identity;
using ShireBudgeters.Common.DTOs;

namespace ShireBudgeters.BL.Services.Identity
{
    internal class IdentityService(SignInManager<IdentityUser> signInManager) : IIdentityService
    {
        public AuthDTOs.UserInfoDTO GetCurrentUserAsync()
        {
            throw new NotImplementedException();
        }

        public AuthDTOs.LoginRequestDTO LoginAsync(AuthDTOs.LoginRequestDTO request)
        {
            throw new NotImplementedException();
        }

        public void LogoutAsync()
        {
            throw new NotImplementedException();
        }
    }
}
