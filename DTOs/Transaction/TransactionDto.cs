namespace SmartSpend.API.DTOs.Transaction
{
    public class TransactionDto
    {
        public int TransactionId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public DateTime TransactionDate { get; set; }
        public string? Notes { get; set; }
        public bool IsRecurring { get; set; }
    }
}