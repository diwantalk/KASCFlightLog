using Microsoft.AspNetCore.Identity;

namespace KASCFlightLog.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public bool IsValidated { get; set; }
        public DateTime? ValidationDate { get; set; }
        public string? ValidatedBy { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}