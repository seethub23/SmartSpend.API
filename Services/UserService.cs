using Microsoft.EntityFrameworkCore;
using SmartSpend.API.Data;
using SmartSpend.API.DTOs;
using SmartSpend.API.Interfaces;

namespace SmartSpend.API.Services
{
    public class UserService : IUserService
    {
        private readonly SmartSpendDbContext _context;

        public UserService(SmartSpendDbContext context)
        {
            _context = context;
        }

        public async Task<UserProfileDto?> GetProfile(int userId)
        {
            return await _context.Users
                .Where(u => u.UserId == userId)
                .Select(u => new UserProfileDto
                {
                    UserId = u.UserId,
                    Name = u.Name,
                    Email = u.Email,
                    CreatedDate = u.CreatedDate
                })
                .FirstOrDefaultAsync();
        }

        public async Task<UserProfileDto> UpdateProfile(
            int userId, UpdateProfileDto dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
                throw new Exception("User not found!");

            // Check if email already taken by another user
            var emailExists = await _context.Users
                .AnyAsync(u => u.Email == dto.Email
                    && u.UserId != userId);

            if (emailExists)
                throw new Exception("Email already in use!");

            user.Name = dto.Name;
            user.Email = dto.Email;

            await _context.SaveChangesAsync();

            return new UserProfileDto
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                CreatedDate = user.CreatedDate
            };
        }

        public async Task<bool> ChangePassword(
            int userId, ChangePasswordDto dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null) return false;

            // Verify current password
            var isValid = BCrypt.Net.BCrypt
                .Verify(dto.CurrentPassword, user.PasswordHash);

            if (!isValid) return false;

            // Hash and save new password
            user.PasswordHash = BCrypt.Net.BCrypt
                .HashPassword(dto.NewPassword);

            await _context.SaveChangesAsync();
            return true;
        }
    }
}