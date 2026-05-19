using SmartSpend.API.DTOs.Budget;

namespace SmartSpend.API.Interfaces
{
    public interface IBudgetService
    {
        Task<List<BudgetDto>> GetAllBudgets(int userId, int month, int year);
        Task<BudgetDto> AddBudget(int userId, AddBudgetDto dto);
        Task<bool> DeleteBudget(int budgetId, int userId);
    }
}