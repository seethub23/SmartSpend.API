namespace SmartSpend.API.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; } = string.Empty; // Income or Expense
        public string PaymentMethod { get; set; } = string.Empty;
        public DateTime TransactionDate { get; set; }
        public string? Notes { get; set; }
        public bool IsRecurring { get; set; }
        public DateTime CreatedDate { get; set; }

        // Navigation properties
        public User User { get; set; } = null!;
        public Category Category { get; set; } = null!;
    }
}