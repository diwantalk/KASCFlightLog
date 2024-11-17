using KASCFlightLog.Models;

namespace KASCFlightLog.ViewModels
{
    public class UserDashboardViewModel
    {
        public IEnumerable<FlightLog> MyFlightLogs { get; set; }
        public IEnumerable<FlightLog> PublishedFlightLogs { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime JoinDate { get; set; }
        public int TotalFlights { get; set; }
        public int IncompleteFlightLogs { get; set; }
        public int PartialFlightLogs { get; set; }
        public int CompleteFlightLogs { get; set; }
        public IEnumerable<Notification> RecentNotifications { get; set; }
        public Dictionary<string, int> FlightsByAircraft { get; set; }
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