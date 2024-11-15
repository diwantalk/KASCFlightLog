namespace KASCFlightLog.Models.ViewModels
{
    public class FlightLogViewModel
    {
        public FlightLog FlightLog { get; set; }
        public List<Staff> AvailablePilots { get; set; }
        public List<string> AircraftTypes { get; set; }
        public List<string> Airports { get; set; }
    }
}