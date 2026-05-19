using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using SmartSpend.API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SmartSpend.API.Helpers
{
    public class JwtHelper
    {
        // IConfiguration is like a dictionary of appsettings
        //Key Value
        //---                          -----
        //JwtSettings:SecretKey    →   "SmartSpendSuperSecretKey123!"
        //JwtSettings:Issuer       →   "SmartSpend"
        //JwtSettings:ExpiryInDays →   "7"
        //ConnectionStrings:DefaultConnection → "Server=localhost;..."


        //"I need a variable called _configuration of type IConfiguration — I'll use it later!"
        //private - Only this class can access it
        //readonly - Can only be assigned once — in constructor! Can't be changed later!
        // Constructor -- runs automatically when JwtHelper is created!
        private readonly IConfiguration _configuration; 
        public JwtHelper(IConfiguration configuration)// .NET injects it here!
        {
            _configuration = configuration; // Assign to field so we can use it anywhere!
        }

        public string GenerateToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");

            // Claims -- data stored inside the token!
            var claims = new List<Claim>
            {
                new Claim("userId", user.UserId.ToString()),
                new Claim("email", user.Email),
                new Claim("name", user.Name)
            };

            // Signing key
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!));

            var credentials = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256);

            // Token expiry
            var expiry = DateTime.UtcNow.AddDays(
                int.Parse(jwtSettings["ExpiryInDays"]!));

            // Create token
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: expiry,
                signingCredentials: credentials
            );

            // Return token string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public int GetUserIdFromToken(ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst("userId");
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
        }
    }
}