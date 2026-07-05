using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace projectcodefirst.Models
{
    
    public class AppDbContext : IdentityDbContext<Users>
    {
        
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

   
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}