using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Kutuphane.Model
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "İsim zorunludur!")]
        [DisplayName("Ad Soyad")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email zorunludur!")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;


    }
}