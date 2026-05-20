using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartSpend.API.Helpers;
using SmartSpend.API.Interfaces;

namespace SmartSpend.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : BaseController
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService, JwtHelper jwtHelper) : base(jwtHelper)
        {
            _reportService = reportService;
        }

        [HttpGet("dashboard-summary")]
        public async Task<IActionResult> GetDashboardSummary([FromQuery] int month, [FromQuery] int year)
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
            {
                return Unauthorized();
            }

            var summary = await _reportService.GetDashboardSummary(userId, month, year);
            return Ok(summary);
        }

        [HttpGet("category-wise-spending")]
        public async Task<IActionResult> GetCategoryWiseSpending([FromQuery] int month, [FromQuery] int year)
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
            {
                return Unauthorized();
            }

            var spending = await _reportService.GetCategoryWiseSpending(userId, month, year);
            return Ok(spending);
        }

        [HttpGet("monthly-trends")]
        public async Task<IActionResult> GetMonthlyTrends([FromQuery] int year)
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
            {
                return Unauthorized();
            }

            var trends = await _reportService.GetMonthlyTrends(userId, year);
            return Ok(trends);
        }
    }
}
