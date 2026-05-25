using SmartSpend.API.DTOs;

namespace SmartSpend.API.Interfaces
{
    public interface IRecurringTransactionService
    {
        Task<List<RecurringTransactionDto>> GetAll(int userId);
        Task<RecurringTransactionDto> Add(int userId, AddRecurringTransactionDto dto);
        Task<bool> ToggleActive(int recurringId, int userId);
        Task<bool> Delete(int recurringId, int userId);
    }
}