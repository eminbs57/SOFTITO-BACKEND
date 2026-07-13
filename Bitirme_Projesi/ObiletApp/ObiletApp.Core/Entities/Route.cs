using ObiletApp.Core.Common;
using System.Collections.Generic;

namespace ObiletApp.Core.Entities
{
    public class Route : BaseEntity
    {
        public int DepartureLocationId { get; set; }
        public int ArrivalLocationId { get; set; }
        public double DistanceInKm { get; set; }

        public Location? DepartureLocation { get; set; }
        public Location? ArrivalLocation { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public string? DepartureLocationName { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public string? ArrivalLocationName { get; set; }

        public ICollection<Trip>? Trips { get; set; }
    }
}