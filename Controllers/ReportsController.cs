using Microsoft.AspNetCore.Mvc;

namespace SmartSpend.API.Controllers
{
    public class ReportsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
