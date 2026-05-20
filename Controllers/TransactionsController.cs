using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartSpend.API.DTOs.Transaction;
using SmartSpend.API.Helpers;
using SmartSpend.API.Interfaces;

namespace SmartSpend.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : BaseController
    {
        private readonly ITransactionService _transactionService;

        public TransactionsController(ITransactionService transactionService, JwtHelper jwtHelper) : base(jwtHelper)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTransactions()
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
            {
                return Unauthorized();
            }

            var transactions = await _transactionService.GetAllTransactions(userId);
            return Ok(transactions);
        }

        [HttpGet("{transactionId:int}")]
        public async Task<IActionResult> GetTransactionById(int transactionId)
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
            {
                return Unauthorized();
            }

            var transaction = await _transactionService.GetTransactionById(transactionId, userId);
            if (transaction == null)
            {
                return NotFound();
            }

            return Ok(transaction);
        }

        [HttpPost]
        public async Task<IActionResult> AddTransaction([FromBody] AddTransactionDto dto)
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
            {
                return Unauthorized();
            }

            var transaction = await _transactionService.AddTransaction(userId, dto);
            return CreatedAtAction(
                nameof(GetTransactionById),
                new { transactionId = transaction.TransactionId },
                transaction);
        }

        [HttpPut("{transactionId:int}")]
        public async Task<IActionResult> UpdateTransaction(int transactionId, [FromBody] AddTransactionDto dto)
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
            {
                return Unauthorized();
            }

            try
            {
                var transaction = await _transactionService.UpdateTransaction(transactionId, userId, dto);
                return Ok(transaction);
            }
            catch (Exception ex) when (ex.Message == "Transaction not found!")
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{transactionId:int}")]
        public async Task<IActionResult> DeleteTransaction(int transactionId)
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
            {
                return Unauthorized();
            }

            try
            {
                await _transactionService.DeleteTransaction(transactionId, userId);
                return NoContent();
            }
            catch (Exception ex) when (ex.Message == "Transaction not found!")
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
