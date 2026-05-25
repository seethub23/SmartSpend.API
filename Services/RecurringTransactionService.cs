using Microsoft.EntityFrameworkCore;
using SmartSpend.API.Data;
using SmartSpend.API.DTOs;
using SmartSpend.API.Interfaces;
using SmartSpend.API.Models;

namespace SmartSpend.API.Services
{
    public class RecurringTransactionService : IRecurringTransactionService
    {
        private readonly SmartSpendDbContext _context;

        public RecurringTransactionService(SmartSpendDbContext context)
        {
            _context = context;
        }

        public async Task<List<RecurringTransactionDto>> GetAll(int userId)
        {
            return await _context.RecurringTransactions
                .Where(r => r.UserId == userId)
                .Include(r => r.Category)
                .OrderByDescending(r => r.StartDate)
                .Select(r => new RecurringTransactionDto
                {
                    RecurringId = r.RecurringId,
                    CategoryName = r.Category.Name,
                    Type = r.Type,
                    Amount = r.Amount,
                    PaymentMethod = r.PaymentMethod,
                    Frequency = r.Frequency,
                    StartDate = r.StartDate,
                    EndDate = r.EndDate,
                    Notes = r.Notes,
                    IsActive = r.IsActive
                })
                .ToListAsync();
        }

        public async Task<RecurringTransactionDto> Add(
            int userId, AddRecurringTransactionDto dto)
        {
            var recurring = new RecurringTransaction
            {
                UserId = userId,
                CategoryId = dto.CategoryId,
                Amount = dto.Amount,
                Type = dto.Type,
                PaymentMethod = dto.PaymentMethod,
                Frequency = dto.Frequency,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Notes = dto.Notes,
                IsActive = true
            };

            _context.RecurringTransactions.Add(recurring);
            await _context.SaveChangesAsync();

            var category = await _context.Categories
                .FindAsync(dto.CategoryId);

            return new RecurringTransactionDto
            {
                RecurringId = recurring.RecurringId,
                CategoryName = category?.Name ?? string.Empty,
                Type = recurring.Type,
                Amount = recurring.Amount,
                PaymentMethod = recurring.PaymentMethod,
                Frequency = recurring.Frequency,
                StartDate = recurring.StartDate,
                EndDate = recurring.EndDate,
                Notes = recurring.Notes,
                IsActive = recurring.IsActive
            };
        }

        public async Task<bool> ToggleActive(int recurringId, int userId)
        {
            var recurring = await _context.RecurringTransactions
                .FirstOrDefaultAsync(r =>
                    r.RecurringId == recurringId &&
                    r.UserId == userId);

            if (recurring == null) return false;

            recurring.IsActive = !recurring.IsActive;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(int recurringId, int userId)
        {
            var recurring = await _context.RecurringTransactions
                .FirstOrDefaultAsync(r =>
                    r.RecurringId == recurringId &&
                    r.UserId == userId);

            if (recurring == null) return false;

            _context.RecurringTransactions.Remove(recurring);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}