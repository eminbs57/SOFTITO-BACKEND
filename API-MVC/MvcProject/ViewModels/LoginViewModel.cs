using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MvcProject.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email zorunludur!")]
        [EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Parola Zorunludur!")]
        public string Password { get; set; }

        [Display(Name = "Hatırla")]
        public bool RememberMe { get; set; }
    }
}