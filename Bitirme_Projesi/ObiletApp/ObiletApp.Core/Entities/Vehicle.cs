using ObiletApp.Core.Common;
using ObiletApp.Core.Enums;

namespace ObiletApp.Core.Entities
{
    public class Vehicle : BaseEntity
    {
        public int CompanyId { get; set; }
        public VehicleType Type { get; set; }
        public int Capacity { get; set; }
        public string? Plate { get; set; }
        public Company? Company { get; set; }

    }
}