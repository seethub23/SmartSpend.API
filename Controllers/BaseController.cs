using Microsoft.AspNetCore.Mvc;
using SmartSpend.API.Helpers;

namespace SmartSpend.API.Controllers
{
    public class BaseController : ControllerBase
    {
        protected readonly JwtHelper _jwtHelper;

        public BaseController(JwtHelper jwtHelper)
        {
            _jwtHelper = jwtHelper;
        }

        protected int GetCurrentUserId()
        {
            return _jwtHelper.GetUserIdFromToken(User);
        }
    }
}