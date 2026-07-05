using System.ComponentModel.DataAnnotations;

namespace projectcodefirst.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public int Quantity { get; set; } 

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}