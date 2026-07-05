using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kutuphane.Model
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Kitap adı gereklidir.")]
        [StringLength(100)]
        [DisplayName("Kitap Adı")]
        public string Title { get; set; } = string.Empty;

        [DisplayName("Sayfa Sayısı")]
        [Range(1, 5000)]
        public int PageCount { get; set; }

        [DisplayName("Yayın Yılı")]
        public int PublishYear { get; set; }


        [Required(ErrorMessage = "Lütfen bir kategori seçiniz.")]
        [DisplayName("Kategori")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }


        [Required(ErrorMessage = "Yazar adı gereklidir.")]
        [DisplayName("Yazar")]
        public string Author { get; set; } = string.Empty;

        public int Stock { get; set; }
    }
}