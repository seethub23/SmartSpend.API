using Microsoft.AspNetCore.Mvc;

namespace SmartSpend.API.Controllers
{
    public class CategoriesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
