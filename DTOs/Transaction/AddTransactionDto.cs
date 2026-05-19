namespace SmartSpend.API.DTOs.Transaction
{
    public class AddTransactionDto
    {
        public int CategoryId { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public DateTime TransactionDate { get; set; }
        public string? Notes { get; set; }
        public bool IsRecurring { get; set; }
    }
}