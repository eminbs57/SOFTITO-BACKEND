using System.ComponentModel.DataAnnotations;

namespace MvcProject.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Email Zorunludur!")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Parola Zorunludur!")]
        [DataType(DataType.Password)]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "Minimum 8 Karakter olmalı")]
        [Compare("ConfirmNewPassword", ErrorMessage = "Parola Eşleşmedi!")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Parola Zorunludur")]
        [DataType(DataType.Password)]
        [Display(Name = "yeni parola eşleşme")]
        public string ConfirmNewPassword { get; set; }

    }
}