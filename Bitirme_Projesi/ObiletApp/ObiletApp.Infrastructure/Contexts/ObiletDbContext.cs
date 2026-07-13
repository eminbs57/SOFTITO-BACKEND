using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ObiletApp.Core.Entities;

namespace ObiletApp.Infrastructure.Contexts
{
    public class ObiletDbContext : IdentityDbContext<AppUser>
    {
        public ObiletDbContext(DbContextOptions<ObiletDbContext> options) : base(options)
        {
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Passenger> Passengers { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Route>()
                .HasOne(r => r.DepartureLocation)
                .WithMany()
                .HasForeignKey(r => r.DepartureLocationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Route>()
                .HasOne(r => r.ArrivalLocation)
                .WithMany()
                .HasForeignKey(r => r.ArrivalLocationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount).HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Trip>()
                .Property(t => t.Price).HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Campaign>()
                .Property(c => c.DiscountPercentage).HasColumnType("decimal(18,2)");
        }
    }
}
