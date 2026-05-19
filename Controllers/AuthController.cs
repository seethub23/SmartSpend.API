using Microsoft.AspNetCore.Mvc;

namespace SmartSpend.API.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
