using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartSpend.API.DTOs.Budget;
using SmartSpend.API.Helpers;
using SmartSpend.API.Interfaces;

namespace SmartSpend.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BudgetsController : BaseController
    {
        private readonly IBudgetService _budgetService;

        public BudgetsController(IBudgetService budgetService, JwtHelper jwtHelper) : base(jwtHelper)
        {
            _budgetService = budgetService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBudgets([FromQuery] int month, [FromQuery] int year)
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
            {
                return Unauthorized();
            }

            var budgets = await _budgetService.GetAllBudgets(userId, month, year);
            return Ok(budgets);
        }

        [HttpPost]
        public async Task<IActionResult> AddBudget([FromBody] AddBudgetDto dto)
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
            {
                return Unauthorized();
            }

            var budget = await _budgetService.AddBudget(userId, dto);
            return CreatedAtAction(nameof(GetAllBudgets), new { month = budget.Month, year = budget.Year }, budget);
        }

        [HttpDelete("{budgetId:int}")]
        public async Task<IActionResult> DeleteBudget(int budgetId)
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
            {
                return Unauthorized();
            }

            var deleted = await _budgetService.DeleteBudget(budgetId, userId);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
