using System.ComponentModel.DataAnnotations;

namespace projectcodefirst.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        
        public List<Product> Products { get; set; }
    }
}