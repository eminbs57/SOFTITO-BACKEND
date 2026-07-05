using System.ComponentModel.DataAnnotations;
namespace MvcProject.ViewModels
{

    public class VerifyEmailViewModel
    {
        [Required(ErrorMessage = "Email zorunludur!")]
        [EmailAddress]
        public string Email { get; set; }
    }
}