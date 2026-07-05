using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace projectcodefirst.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Email zorunludur")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Parola zorunludur")]
        [DataType(DataType.Password)]
        [StringLength(40,MinimumLength = 4,ErrorMessage ="Minimum 4 karakter")]
        [Compare("ConfirmNewPassword",ErrorMessage ="Parola eşleşmedi")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Parola zorunludur")]
        [DataType(DataType.Password)]
        [Display(Name ="Yeni parola eşleşme")]
        public string ConfirmNewPassword { get; set; }
    }
}
