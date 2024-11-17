using KASCFlightLog.Models;

namespace KASCFlightLog.ViewModels
{
    public class AdminDashboardViewModel
    {
        public IEnumerable<ApplicationUser> PendingUsers { get; set; }
        public IEnumerable<FlightLog> PendingFlightLogs { get; set; }
        public IEnumerable<FlightLog> RecentFlightLogs { get; set; }
        public int TotalUsers { get; set; }
        public int TotalFlightLogs { get; set; }
        public int PendingApprovals { get; set; }
        public IEnumerable<Notification> RecentNotifications { get; set; }

        public AdminDashboardViewModel()
        {
            PendingUsers = new List<ApplicationUser>();
            PendingFlightLogs = new List<FlightLog>();
            RecentFlightLogs = new List<FlightLog>();
            RecentNotifications = new List<Notification>();
        }
    }
}