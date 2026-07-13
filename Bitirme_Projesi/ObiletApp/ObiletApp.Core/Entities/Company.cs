using ObiletApp.Core.Common;
using ObiletApp.Core.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ObiletApp.Core.Entities
{
    public class Company : BaseEntity
    {
        [Required(ErrorMessage = "Firma adı zorunludur.")]
        public string Name { get; set; } = string.Empty;

        public string? ContactInfo { get; set; }
        public string? LogoUrl { get; set; }

        [Required(ErrorMessage = "Firma tipi zorunludur.")]
        public VehicleType Type { get; set; }

        public ICollection<Vehicle>? Vehicles { get; set; }
    }
}