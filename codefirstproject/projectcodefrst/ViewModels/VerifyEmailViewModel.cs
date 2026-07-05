using System.ComponentModel.DataAnnotations;

namespace projectcodefirst.ViewModels
{
    public class VerifyEmailViewModel
    {

        [Required(ErrorMessage = "Email zorunludur")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
