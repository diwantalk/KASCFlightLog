using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace KASCFlightLog.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string IDNumber { get; set; }
    }
}
