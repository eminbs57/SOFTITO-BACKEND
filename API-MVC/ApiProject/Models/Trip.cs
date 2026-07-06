using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiProject.Models
{
    public class Trip
    {
        public int Id { get; set; }

        [Display(Name = "Sefer Tarihi")]
        public DateTime TripDate { get; set; }

        public int RouteId { get; set; }
        [ForeignKey("RouteId")]
        public ApiProject.Models.Route? Route { get; set; }

        public int DriverId { get; set; }
        [ForeignKey("DriverId")]
        public Driver? Driver { get; set; }

        public int VehicleId { get; set; }
        [ForeignKey("VehicleId")]
        public Vehicle? Vehicle { get; set; }
    }
}
