using SmartSpend.API.DTOs;

namespace SmartSpend.API.Interfaces
{
    public interface IUserService
    {
        Task<UserProfileDto?> GetProfile(int userId);
        Task<UserProfileDto> UpdateProfile(int userId, UpdateProfileDto dto);
        Task<bool> ChangePassword(int userId, ChangePasswordDto dto);
    }
}