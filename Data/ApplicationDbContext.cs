using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using KASCFlightLog.Models;

namespace KASCFlightLog.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Staff> Staff { get; set; }
        public DbSet<FlightLog> FlightLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure your model relationships and constraints here
            modelBuilder.Entity<Staff>(entity =>
            {
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.IDNumber).IsUnique();
            });

            modelBuilder.Entity<FlightLog>(entity =>
            {
                entity.HasIndex(e => e.RegistrationNO);
                entity.Property(e => e.IsValid).HasDefaultValue(false);
                entity.Property(e => e.IsPublished).HasDefaultValue(false);
            });
        }
    }
}
