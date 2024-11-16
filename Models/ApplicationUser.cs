using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace KASCFlightLog.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public UserRole Role { get; set; } = UserRole.User;
        public ICollection<FlightLog> FlightLogs { get; set; }
    }
    public enum UserRole
    {
        User,
        Staff,
        Admin
    }
}
