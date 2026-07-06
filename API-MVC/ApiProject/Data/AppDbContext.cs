using Microsoft.EntityFrameworkCore;
using ApiProject.Models;

namespace ApiProject.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Depo> Depos { get; set; }
        public DbSet<ApiProject.Models.Route> Routes { get; set; }
        public DbSet<Trip> Trips { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApiProject.Models.Route>()
                .HasOne(r => r.StartDepo)
                .WithMany()
                .HasForeignKey(r => r.StartDepoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ApiProject.Models.Route>()
                .HasOne(r => r.EndDepo)
                .WithMany()
                .HasForeignKey(r => r.EndDepoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Trip>()
                .HasOne(t => t.Driver)
                .WithMany(d => d.Trips)
                .HasForeignKey(t => t.DriverId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Trip>()
                .HasOne(t => t.Vehicle)
                .WithMany(v => v.Trips)
                .HasForeignKey(t => t.VehicleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Trip>()
                .HasOne(t => t.Route)
                .WithMany(r => r.Trips)
                .HasForeignKey(t => t.RouteId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
