namespace SmartSpend.API.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public DateTime CreatedDate { get; set; }

        // Navigation properties
        public User User { get; set; } = null!;
    }
}