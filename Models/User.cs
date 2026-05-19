using SmartSpend.API.DTOs;

namespace SmartSpend.API.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }

        // Navigation properties
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public ICollection<Budget> Budgets { get; set; } = new List<Budget>();
        public ICollection<RecurringTransaction> RecurringTransactions { get; set; } = new List<RecurringTransaction>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}