using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KASCFlightLog.Models
{
    public class FlightLog
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        public string? ValidatedById { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        [ForeignKey("ValidatedById")]
        public ApplicationUser? ValidatedBy { get; set; }

        [StringLength(20)]
        public string RegistrationNO { get; set; }

        [Required]
        [Display(Name = "Pilot In Command")]
        [StringLength(100)]
        public string PilotInCommand { get; set; }

        [Display(Name = "P3/PAX")]
        [StringLength(100)]
        public string P3PAX { get; set; }

        [Required]
        [Display(Name = "From")]
        [StringLength(50)]
        public string From { get; set; }

        [Required]
        [Display(Name = "To")]
        [StringLength(50)]
        public string To { get; set; }

        [Required]
        [Display(Name = "Authorized By")]
        [StringLength(100)]
        public string AuthorizedBy { get; set; }

        [Display(Name = "Remarks")]
        [StringLength(500)]
        public string Remarks { get; set; }

        [Display(Name = "Time Departure")]
        public TimeSpan? TimeDeparture { get; set; }

        [Display(Name = "Time Arrival")]
        public TimeSpan? TimeArrival { get; set; }

        [Display(Name = "Number Of Landings")]
        [Range(0, 100)]
        public int? NumberOfLandings { get; set; }

        [Display(Name = "Duration")]
        public TimeSpan? TimeDuration { get; set; }

        [Display(Name = "Note")]
        [StringLength(1000)]
        public string Note { get; set; }

        [Display(Name = "Is Valid")]
        public bool IsValid { get; set; }

        [Display(Name = "Is Published")]
        public bool IsPublished { get; set; }

       
        // Audit Properties
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? ValidatedAt { get; set; }
        public DateTime? PublishedAt { get; set; }

        // Computed Properties
        [Display(Name = "Status")]
        public string Status => GetStatus();

        private string GetStatus()
        {
            if (!TimeDeparture.HasValue && !TimeArrival.HasValue &&
                !NumberOfLandings.HasValue && !TimeDuration.HasValue)
                return "Incomplete";

            if (TimeDeparture.HasValue &&
                (!TimeArrival.HasValue || !NumberOfLandings.HasValue || !TimeDuration.HasValue))
                return "Partial";

            return "Complete";
        }

        // Helper method to set PilotInCommand from User
        public void SetPilotInCommandFromUser(ApplicationUser user)
        {
            if (user != null && !string.IsNullOrEmpty(user.FirstName) && !string.IsNullOrEmpty(user.LastName))
            {
                PilotInCommand = $"{user.FirstName} {user.LastName}";
            }
        }

        // Helper method to calculate duration
        public void CalculateDuration()
        {
            if (TimeDeparture.HasValue && TimeArrival.HasValue)
            {
                if (TimeArrival.Value >= TimeDeparture.Value)
                {
                    TimeDuration = TimeArrival.Value - TimeDeparture.Value;
                }
                else
                {
                    // Handle case where flight crosses midnight
                    TimeDuration = (TimeSpan.FromHours(24) - TimeDeparture.Value) + TimeArrival.Value;
                }
            }
        }
    }
}