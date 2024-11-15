using System.ComponentModel.DataAnnotations;

namespace KASCFlightLog.Models
{
    public class Staff
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "ID Number")]
        [StringLength(20)]
        public string IDNumber { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(256)]
        public string Email { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        [StringLength(20)]
        public string Phone { get; set; }

        // Additional Properties
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Position")]
        [StringLength(100)]
        public string Position { get; set; }

        // Audit Properties
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public string LastModifiedBy { get; set; }
    }
}
