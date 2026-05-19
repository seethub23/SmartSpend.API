using Microsoft.AspNetCore.Mvc;

namespace SmartSpend.API.Controllers
{
    public class BudgetsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
