using System.ComponentModel.DataAnnotations;

namespace MvcProject.ViewModels
{


    public class RegisterViewModel
    {

        [Required(ErrorMessage = "isim zorunludur!")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Email zorunludur!")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Parola zorunludur!")]
        [DataType(DataType.Password)]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "Minimum 8 karakter olmalıdır!")]
        [Compare("ConfirmPassword", ErrorMessage = "Parola eşleşmedi!")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Parola Zorunludur!")]
        [DataType(DataType.Password)]
        [Display(Name = "Yeni parola eşleşme")]
        public string ConfirmPassword { get; set; }
    }
}
