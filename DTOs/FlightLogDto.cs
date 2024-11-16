namespace KASCFlightLog.DTOs
{
    public class FlightLogDto
    {
        public DateTime FlightDate { get; set; }
        public string AircraftRegistration { get; set; }
        public string DepartureLocation { get; set; }
        public string ArrivalLocation { get; set; }
    }
}