namespace SmartSpend.API.Interfaces
{
    public interface IReportService
    {
        Task<object> GetDashboardSummary(int userId, int month, int year);
        Task<object> GetCategoryWiseSpending(int userId, int month, int year);
        Task<object> GetMonthlyTrends(int userId, int year);
    }
}