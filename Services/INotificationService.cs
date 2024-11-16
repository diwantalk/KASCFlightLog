using KASCFlightLog.Models;

namespace KASCFlightLog.Services
{
    public interface INotificationService
    {
        Task NotifyUser(string userId, string title, string message);
        Task NotifyAdminsAndStaff(string title, string message);
        Task<IEnumerable<Notification>> GetUnreadNotifications(string userId);
        Task MarkAsRead(int notificationId);
    }
}