using SmartSpend.API.DTOs;

namespace SmartSpend.API.Interfaces
{
    public interface INotificationService
    {
        Task<List<NotificationDto>> GetNotifications(int userId);
        Task<bool> MarkAsRead(int notificationId, int userId);
        Task<bool> MarkAllAsRead(int userId);
        Task<int> GetUnreadCount(int userId);
    }
}