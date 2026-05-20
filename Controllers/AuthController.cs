using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartSpend.API.DTOs.Auth;
using SmartSpend.API.Helpers;
using SmartSpend.API.Interfaces;

namespace SmartSpend.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService, JwtHelper jwtHelper) : base(jwtHelper)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var result = await _authService.Register(registerDto);
            if (result == "User Already Exists")
            {
                return BadRequest(new { message = result });
            }

            return Ok(new { token = result });
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var result = await _authService.Login(loginDto);
            if (result == "Invalid email or password!")
            {
                return Unauthorized(new { message = result });
            }

            return Ok(new { token = result });
        }
    }
}
