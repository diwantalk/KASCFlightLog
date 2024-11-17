namespace KASCFlightLog.DTOs
{
    public class FlightLogDto
    {
        public DateTime FlightDate { get; set; }
        public string RegistrationNO { get; set; }
        public string PilotInCommand { get; set; }
        public string P3PAX { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string AuthorizedBy { get; set; }
        public string Remarks { get; set; }
        public TimeSpan? TimeDeparture { get; set; }
        public TimeSpan? TimeArrival { get; set; }
        public int? NumberOfLandings { get; set; }
        public string Note { get; set; }
    }
}