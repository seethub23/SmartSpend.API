namespace SmartSpend.API.DTOs.Budget
{
    public class AddBudgetDto
    {
        public int CategoryId { get; set; }
        public decimal LimitAmount { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }
}