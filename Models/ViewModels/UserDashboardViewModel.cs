using System.Collections.Generic;
using KASCFlightLog.Models;

namespace KASCFlightLog.ViewModels
{
    public class UserDashboardViewModel
    {
        // Collection of user's own flight logs
        public IEnumerable<FlightLog> MyFlightLogs { get; set; }

        // Collection of all published flight logs
        public IEnumerable<FlightLog> PublishedFlightLogs { get; set; }

        // User profile information
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime JoinDate { get; set; }
        public int TotalFlights { get; set; }

        // Statistics
        public int PendingFlightLogs { get; set; }
        public int ApprovedFlightLogs { get; set; }
        public int RejectedFlightLogs { get; set; }

        // Recent notifications
        public IEnumerable<Notification> RecentNotifications { get; set; }

        // Aircraft statistics
        public Dictionary<string, int> FlightsByAircraft { get; set; }

        // Monthly statistics
        public Dictionary<string, int> FlightsByMonth { get; set; }

        public UserDashboardViewModel()
        {
            MyFlightLogs = new List<FlightLog>();
            PublishedFlightLogs = new List<FlightLog>();
            RecentNotifications = new List<Notification>();
            FlightsByAircraft = new Dictionary<string, int>();
            FlightsByMonth = new Dictionary<string, int>();
        }
    }
}