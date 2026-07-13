using System;

namespace ObiletApp.API.Dapper.Models
{
    public class TripAdminListDto
    {
        public int Id { get; set; }
        public string RouteName { get; set; }
        public string CompanyName { get; set; }
        public string VehiclePlate { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public decimal Price { get; set; }
    }
}
