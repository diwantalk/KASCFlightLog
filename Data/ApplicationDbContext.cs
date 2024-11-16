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

            modelBuilder.Entity<FlightLog>()
            .HasOne(f => f.User)
            .WithMany(u => u.CreatedFlightLogs)
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FlightLog>()
            .HasOne(f => f.ValidatedBy)
            .WithMany(u => u.ValidatedFlightLogs)
            .HasForeignKey(f => f.ValidatedById)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
