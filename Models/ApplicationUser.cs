using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KASCFlightLog.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "ID Number")]
        public string IdNumber { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        // Navigation Properties
        public virtual ICollection<FlightLog> CreatedFlightLogs { get; set; }
        public virtual ICollection<FlightLog> ValidatedFlightLogs { get; set; }

        // Computed Properties
        [NotMapped]
        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {LastName}";

        // Audit Properties
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastModifiedAt { get; set; }

        public ApplicationUser()
        {
            CreatedFlightLogs = new HashSet<FlightLog>();
            ValidatedFlightLogs = new HashSet<FlightLog>();
        }
    }
}