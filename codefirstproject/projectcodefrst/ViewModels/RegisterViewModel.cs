using System.ComponentModel.DataAnnotations;

namespace projectcodefirst.ViewModels
{
    public class RegisterViewModel
    {

        [Required(ErrorMessage = "İsim zorunludur")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email zorunludur")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Parola zorunludur")]
        [DataType(DataType.Password)]
        [StringLength(40, MinimumLength = 4, ErrorMessage = "Minimum 4 karakter")]
        [Compare("ConfirmPassword", ErrorMessage = "Parola eşleşmedi")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Parola zorunludur")]
        [DataType(DataType.Password)]
        [Display(Name = "Yeni parola eşleşme")]
        public string ConfirmPassword { get; set; }
    }
}
