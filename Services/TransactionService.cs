using Microsoft.EntityFrameworkCore;
using SmartSpend.API.Data;
using SmartSpend.API.DTOs.Transaction;
using SmartSpend.API.Interfaces;
using SmartSpend.API.Models;

namespace SmartSpend.API.Services
{
    public class TransactionService: ITransactionService
    {
        private readonly SmartSpendDbContext _dbContext;
        public TransactionService(SmartSpendDbContext smartSpendDbContext) {
            _dbContext = smartSpendDbContext;
        }

        public  async Task<List<TransactionDto>> GetAllTransactions(int userId)
        {
            var transactions = await _dbContext.Transactions.Where(x => x.UserId == userId)
                                .Include(t => t.Category)
                                .Select(u => new TransactionDto
                                {
                                    TransactionId = u.TransactionId,
                                    CategoryName = u.Category.Name,
                                    Type = u.Type,
                                    Amount = u.Amount,
                                    PaymentMethod = u.PaymentMethod,
                                    TransactionDate = u.TransactionDate,
                                    IsRecurring = u.IsRecurring,
                                    Notes = u.Notes,
                                })
                                .ToListAsync();
            return transactions;
        }

        public async Task<TransactionDto?> GetTransactionById(int transactionId, int userId)
        {
            var transaction = await _dbContext.Transactions
                .Where(x => x.TransactionId == transactionId && x.UserId == userId)
                .Include(t => t.Category)
               .Select(u => new TransactionDto
               {
                   TransactionId = u.TransactionId,
                   CategoryName = u.Category.Name,
                   Type = u.Type,
                   Amount = u.Amount,
                   PaymentMethod = u.PaymentMethod,
                   TransactionDate = u.TransactionDate,
                   IsRecurring = u.IsRecurring,
                   Notes = u.Notes,
               }).FirstOrDefaultAsync();
                return transaction;
        }

        public async Task<TransactionDto> AddTransaction(int userId, AddTransactionDto dto)
        {
            var newTransaction = new Transaction
            {
                UserId = userId,
                CategoryId = dto.CategoryId,
                Type = dto.Type,
                Amount = dto.Amount,
                PaymentMethod = dto.PaymentMethod,
                TransactionDate = dto.TransactionDate,
                IsRecurring = dto.IsRecurring,
                Notes = dto.Notes,
                CreatedDate = DateTime.UtcNow
            };
            _dbContext.Transactions.Add(newTransaction);
            await _dbContext.SaveChangesAsync();

            var category = _dbContext.Categories.Where(x => x.CategoryId == dto.CategoryId).FirstOrDefault();
            return new TransactionDto
            {
                TransactionId = newTransaction.TransactionId,
                CategoryName = category?.Name ?? string.Empty,
                Type = newTransaction.Type,
                Amount = newTransaction.Amount,
                PaymentMethod = newTransaction.PaymentMethod,
                TransactionDate = newTransaction.TransactionDate,
                Notes = newTransaction.Notes,
                IsRecurring = newTransaction.IsRecurring
            };
        }

        public async Task<TransactionDto> UpdateTransaction(int transactionId, int userId, AddTransactionDto dto)
        {
            var transaction = await _dbContext.Transactions
                .FirstOrDefaultAsync(t => t.TransactionId == transactionId && t.UserId == userId);
            if (transaction == null)
            {
                throw new Exception("Transaction not found!");
            }
            transaction.CategoryId = dto.CategoryId;
            transaction.Amount = dto.Amount;
            transaction.Type = dto.Type;
            transaction.PaymentMethod = dto.PaymentMethod;
            transaction.TransactionDate = dto.TransactionDate;
            transaction.Notes = dto.Notes;
            transaction.IsRecurring = dto.IsRecurring;

            await _dbContext.SaveChangesAsync();
            var category = await _dbContext.Categories.FindAsync(dto.CategoryId);
            return new TransactionDto
            {
                TransactionId = transaction.TransactionId,
                CategoryName = category?.Name ?? string.Empty,
                Type = transaction.Type,
                Amount = transaction.Amount,
                PaymentMethod = transaction.PaymentMethod,
                TransactionDate = transaction.TransactionDate,
                Notes = transaction.Notes,
                IsRecurring = transaction.IsRecurring
            };

        }

        public async Task<bool> DeleteTransaction(int transactionId, int userId)
        {
            var transaction = _dbContext.Transactions.Where(x => x.TransactionId == transactionId && x.UserId == userId).FirstOrDefault();
            if (transaction == null)
            {
                throw new Exception("Transaction not found!");
            }

            _dbContext.Transactions.Remove(transaction);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
