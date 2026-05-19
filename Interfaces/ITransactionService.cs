using SmartSpend.API.DTOs.Transaction;

namespace SmartSpend.API.Interfaces
{
    public interface ITransactionService
    {
        Task<List<TransactionDto>> GetAllTransactions(int userId);
        Task<TransactionDto?> GetTransactionById(int transactionId, int userId);
        Task<TransactionDto> AddTransaction(int userId, AddTransactionDto dto);
        Task<TransactionDto> UpdateTransaction(int transactionId, int userId, AddTransactionDto dto);
        Task<bool> DeleteTransaction(int transactionId, int userId);
    }
}