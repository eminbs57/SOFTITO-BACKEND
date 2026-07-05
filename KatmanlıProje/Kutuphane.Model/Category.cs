using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Kutuphane.Model
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Kategori adı bos gecilemez!")]
        [StringLength(50)]
        [DisplayName("Kategori Adı")]
        public string Name { get; set; } = string.Empty;




        [StringLength(200)]
        [DisplayName("Açıklama")]
        public string? Description { get; set; } = string.Empty;
    }
}