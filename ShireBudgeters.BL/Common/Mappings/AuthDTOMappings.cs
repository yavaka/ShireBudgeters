using ShireBudgeters.Common.DTOs;
using ShireBudgeters.DA.Models;

namespace ShireBudgeters.BL.Common.Mappings;

internal static class AuthDTOMappings
{
    public static AuthDTOs.UserInfoDTO ToUserInfoDTO(this UserModel user) 
        => new(
            user.Id,
            user.Email!,
            user.FirstName!,
            user.LastName!);
}
