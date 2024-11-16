using KASCFlightLog.Models;

namespace KASCFlightLog.ViewModels
{
    public class AdminDashboardViewModel
    {
        public IEnumerable<ApplicationUser> PendingUsers { get; set; }
        public IEnumerable<FlightLog> PendingFlightLogs { get; set; }
        public IEnumerable<Notification> RecentNotifications { get; set; }
    }
}