using System.ComponentModel.DataAnnotations;

namespace MvcProject.Models
{
    public class Depo
    {
        public int Id { get; set; }

        [Display(Name = "Depo Adı")]
        public string DepoName { get; set; } = string.Empty;

        [Display(Name = "Şehir")]
        public string City { get; set; } = string.Empty;

        [Display(Name = "Hacim")]
        public double Volume { get; set; }
    }
}
