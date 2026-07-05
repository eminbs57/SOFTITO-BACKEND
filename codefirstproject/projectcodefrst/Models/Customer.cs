using System.ComponentModel.DataAnnotations;

namespace projectcodefirst.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

    
        public List<Order> Orders { get; set; }
    }
}