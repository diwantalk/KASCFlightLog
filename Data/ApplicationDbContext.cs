using KASCFlightLog.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KASCFlightLog.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<FlightLog> FlightLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure the FlightLog relationships
            builder.Entity<FlightLog>()
                .HasOne(f => f.User)
                .WithMany()
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<FlightLog>()
                .HasOne(f => f.ValidatedBy)
                .WithMany()
                .HasForeignKey(f => f.ValidatedById)
                .OnDelete(DeleteBehavior.Restrict); // Change to Restrict instead of Cascade
        }
    }
}