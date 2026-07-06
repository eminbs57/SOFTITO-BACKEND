using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiProject.Models
{
    public class Route
    {
        public int Id { get; set; }

        [Display(Name = "Rota Adı")]
        public string RouteName { get; set; } = string.Empty;

        [Display(Name = "Mesafe (km)")]
        public double Distance { get; set; }

        public int StartDepoId { get; set; }
        [ForeignKey("StartDepoId")]
        public Depo? StartDepo { get; set; }

        public int EndDepoId { get; set; }
        [ForeignKey("EndDepoId")]
        public Depo? EndDepo { get; set; }
    }
}
