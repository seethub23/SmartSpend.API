using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartSpend.API.DTOs;
using SmartSpend.API.Helpers;
using SmartSpend.API.Interfaces;

namespace SmartSpend.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RecurringTransactionsController : BaseController
    {
        private readonly IRecurringTransactionService _recurringService;

        public RecurringTransactionsController(
            IRecurringTransactionService recurringService,
            JwtHelper jwtHelper) : base(jwtHelper)
        {
            _recurringService = recurringService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = GetCurrentUserId();
            if (userId == 0) return Unauthorized();

            var result = await _recurringService.GetAll(userId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add(
            [FromBody] AddRecurringTransactionDto dto)
        {
            var userId = GetCurrentUserId();
            if (userId == 0) return Unauthorized();

            var result = await _recurringService.Add(userId, dto);
            return Ok(result);
        }

        [HttpPut("{id}/toggle")]
        public async Task<IActionResult> ToggleActive(int id)
        {
            var userId = GetCurrentUserId();
            if (userId == 0) return Unauthorized();

            var result = await _recurringService.ToggleActive(id, userId);
            if (!result) return NotFound();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetCurrentUserId();
            if (userId == 0) return Unauthorized();

            var result = await _recurringService.Delete(id, userId);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}