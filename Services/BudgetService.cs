using Microsoft.EntityFrameworkCore;
using SmartSpend.API.Data;
using SmartSpend.API.DTOs.Budget;
using SmartSpend.API.Interfaces;
using SmartSpend.API.Models;

namespace SmartSpend.API.Services
{
    public class BudgetService : IBudgetService
    {
        private readonly SmartSpendDbContext _context;

        public BudgetService(SmartSpendDbContext context)
        {
            _context = context;
        }

        public async Task<List<BudgetDto>> GetAllBudgets(int userId, int month, int year)
        {
            var budgets = await _context.Budgets
                .Where(b => b.UserId == userId && b.Month == month && b.Year == year)
                .Include(b => b.Category)
                .ToListAsync();

            var result = new List<BudgetDto>();

            foreach (var budget in budgets)
            {
                // Calculate spent amount for this category this month
                var spentAmount = await _context.Transactions
                    .Where(t => t.UserId == userId
                        && t.CategoryId == budget.CategoryId
                        && t.Type == "Expense"
                        && t.TransactionDate.Month == month
                        && t.TransactionDate.Year == year)
                    .SumAsync(t => t.Amount);

                var remainingAmount = budget.LimitAmount - spentAmount;
                var spentPercentage = (int)((spentAmount / budget.LimitAmount) * 100);

                result.Add(new BudgetDto
                {
                    BudgetId = budget.BudgetId,
                    CategoryName = budget.Category.Name,
                    LimitAmount = budget.LimitAmount,
                    SpentAmount = spentAmount,
                    RemainingAmount = remainingAmount,
                    Month = budget.Month,
                    Year = budget.Year,
                    SpentPercentage = spentPercentage
                });
            }

            return result;
        }

        public async Task<BudgetDto> AddBudget(int userId, AddBudgetDto dto)
        {
            var budget = new Budget
            {
                UserId = userId,
                CategoryId = dto.CategoryId,
                LimitAmount = dto.LimitAmount,
                Month = dto.Month,
                Year = dto.Year,
                CreatedDate = DateTime.UtcNow
            };

            _context.Budgets.Add(budget);
            await _context.SaveChangesAsync();

            var category = await _context.Categories.FindAsync(dto.CategoryId);

            return new BudgetDto
            {
                BudgetId = budget.BudgetId,
                CategoryName = category?.Name ?? string.Empty,
                LimitAmount = budget.LimitAmount,
                SpentAmount = 0,
                RemainingAmount = budget.LimitAmount,
                Month = budget.Month,
                Year = budget.Year,
                SpentPercentage = 0
            };
        }

        public async Task<bool> DeleteBudget(int budgetId, int userId)
        {
            var budget = await _context.Budgets
                .FirstOrDefaultAsync(b => b.BudgetId == budgetId && b.UserId == userId);

            if (budget == null)
                return false;

            _context.Budgets.Remove(budget);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}