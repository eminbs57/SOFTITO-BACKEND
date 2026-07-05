using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EventManagement.Models;

namespace EventManagement.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // public DbSet<AppUser> AppUsers { get; set; } // IdentityDbContext already provides Users
        public DbSet<Event> Events { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Konser" },
                new Category { Id = 2, Name = "Tiyatro" },
                new Category { Id = 3, Name = "Seminer" }
            );

            modelBuilder.Entity<Event>().HasData(
                new Event { Id = 1, Title = "Yaz Konseri", CategoryName = "Konser", Date = new DateTime(2026, 7, 10), Description = "Üniversite bahar şenlikleri kapsamında yaz konseri.", Kota = 500 },
                new Event { Id = 2, Title = "Yazılım Mimarlığı Semineri", CategoryName = "Seminer", Date = new DateTime(2026, 6, 25), Description = "Sektörden uzmanların katılımıyla yazılım mimarisi semineri.", Kota = 100 },
                new Event { Id = 3, Title = "Romeo ve Juliet", CategoryName = "Tiyatro", Date = new DateTime(2026, 7, 15), Description = "Tiyatro kulübünün sahneleyeceği ünlü oyun.", Kota = 50 }
            );

            // Configure Many-to-Many relationship between AppUser and Event
            modelBuilder.Entity<AppUser>()
                .HasMany(u => u.Events)
                .WithMany(e => e.Attendees)
                .UsingEntity(j => j.ToTable("AppUserEvents"));
        }
    }
}