using Microsoft.EntityFrameworkCore;
using SmartSpend.API.Data;
using SmartSpend.API.Interfaces;

namespace SmartSpend.API.Services
{
    public class ReportService : IReportService
    {
        private readonly SmartSpendDbContext _context;

        public ReportService(SmartSpendDbContext context)
        {
            _context = context;
        }

        public async Task<object> GetDashboardSummary(int userId, int month, int year)
        {
            var transactions = await _context.Transactions
                .Where(t => t.UserId == userId
                    && t.TransactionDate.Month == month
                    && t.TransactionDate.Year == year)
                .ToListAsync();

            var totalIncome = transactions
                .Where(t => t.Type == "Income")
                .Sum(t => t.Amount);

            var totalExpense = transactions
                .Where(t => t.Type == "Expense")
                .Sum(t => t.Amount);

            var totalSavings = transactions
                .Where(t => t.Type == "Savings")
                .Sum(t => t.Amount);

            return new
            {
                TotalIncome = totalIncome,
                TotalExpense = totalExpense,
                TotalSavings = totalSavings,
                NetBalance = totalIncome - totalExpense - totalSavings,
                Month = month,
                Year = year
            };
        }

        public async Task<object> GetCategoryWiseSpending(int userId, int month, int year)
        {
            var categorySpending = await _context.Transactions
                .Where(t => t.UserId == userId
                    && t.Type == "Expense"
                    && t.TransactionDate.Month == month
                    && t.TransactionDate.Year == year)
                .Include(t => t.Category)
                .GroupBy(t => t.Category.Name)
                .Select(g => new
                {
                    Category = g.Key,
                    TotalAmount = g.Sum(t => t.Amount),
                    TransactionCount = g.Count()
                })
                .OrderByDescending(x => x.TotalAmount)
                .ToListAsync();

            return categorySpending;
        }

        public async Task<object> GetMonthlyTrends(int userId, int year)
        {
            var monthlyTrends = await _context.Transactions
                .Where(t => t.UserId == userId
                    && t.TransactionDate.Year == year)
                .GroupBy(t => new { t.TransactionDate.Month, t.Type })
                .Select(g => new
                {
                    Month = g.Key.Month,
                    Type = g.Key.Type,
                    TotalAmount = g.Sum(t => t.Amount)
                })
                .OrderBy(x => x.Month)
                .ToListAsync();

            return monthlyTrends;
        }
    }
}