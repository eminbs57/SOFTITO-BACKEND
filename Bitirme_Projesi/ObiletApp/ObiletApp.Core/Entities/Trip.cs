using ObiletApp.Core.Common;
using System;
using System.Collections.Generic;

namespace ObiletApp.Core.Entities
{
    public class Trip : BaseEntity
    {
        public int RouteId { get; set; }
        public int VehicleId { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public decimal Price { get; set; }

        public Route? Route { get; set; }
        public Vehicle? Vehicle { get; set; }
        public ICollection<Ticket>? Tickets { get; set; }
        public ICollection<Review>? Reviews { get; set; }
    }
}