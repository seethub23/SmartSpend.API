namespace SmartSpend.API.DTOs.Budget
{
    public class BudgetDto
    {
        public int BudgetId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public decimal LimitAmount { get; set; }
        public decimal SpentAmount { get; set; }
        public decimal RemainingAmount { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int SpentPercentage { get; set; }
    }
}