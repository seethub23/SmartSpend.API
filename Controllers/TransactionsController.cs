using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartSpend.API.Helpers;
using SmartSpend.API.Interfaces;

namespace SmartSpend.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly JwtHelper _jwtHelper;

        public TransactionsController(ITransactionService transactionService, JwtHelper jwtHelper)
        {
            _transactionService = transactionService;
            _jwtHelper = jwtHelper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTransactions()
        {
            var userId = _jwtHelper.GetUserIdFromToken(User);
            if (userId == 0)
            {
                return Unauthorized();
            }

            var transactions = await _transactionService.GetAllTransactions(userId);
            return Ok(transactions);
        }
    }
}
