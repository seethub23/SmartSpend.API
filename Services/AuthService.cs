 using Microsoft.EntityFrameworkCore;
using SmartSpend.API.Data;
using SmartSpend.API.DTOs.Auth;
using SmartSpend.API.Helpers;
using SmartSpend.API.Interfaces;
using SmartSpend.API.Models;
namespace SmartSpend.API.Services
{
    public class AuthService: IAuthService
    {
        private readonly SmartSpendDbContext _dbContext;
        private readonly JwtHelper _jwtHelper;
        public AuthService(SmartSpendDbContext smartSpendDbContext, JwtHelper jwtHelper) {
            _dbContext = smartSpendDbContext;
            _jwtHelper = jwtHelper;
        } 

        public async Task<string> Register(RegisterDto registerDto)
        {
            var existingUser = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == registerDto.Email);
            if(existingUser != null)
            {
                return "User Already Exists";
            }
            // Hash the password
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);
            var user = new User
            {
                Email = registerDto.Email,
                Name = registerDto.Name,
                PasswordHash = passwordHash,
                CreatedDate = DateTime.UtcNow,
                IsActive = true,
            };
            _dbContext.Users.Add(user);
            await  _dbContext.SaveChangesAsync();
            return _jwtHelper.GenerateToken(user);
        }

        public async Task<string> Login(LoginDto loginDto)
        {
            // Find user by email
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email && u.IsActive);

            if (user == null)
                return "Invalid email or password!";

            // Verify password
            var isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);

            if (!isPasswordValid)
                return "Invalid email or password!";

            // Generate and return token
            return _jwtHelper.GenerateToken(user);
        }
    }
}
