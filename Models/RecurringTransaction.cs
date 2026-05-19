namespace SmartSpend.API.Models
{
    public class RecurringTransaction
    {
        public int RecurringId { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public string Frequency { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Notes { get; set; }
        public bool IsActive { get; set; }

        // Navigation properties
        public User User { get; set; } = null!;
        public Category Category { get; set; } = null!;
    }
}