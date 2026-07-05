using System.ComponentModel.DataAnnotations;

namespace ApiProject.Models
{
    public class Driver
    {
        public int Id { get; set; }
        
        [Display(Name = "Ad")]
        public string FirstName { get; set; } = string.Empty;
        
        [Display(Name = "Soyad")]
        public string LastName { get; set; } = string.Empty;
        
        [Display(Name = "Ehliyet Sınıfı")]
        public string LicenseClass { get; set; } = string.Empty;
        
        [Display(Name = "Telefon")]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
