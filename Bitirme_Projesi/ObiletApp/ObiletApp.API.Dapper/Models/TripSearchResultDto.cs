using System;

namespace ObiletApp.API.Dapper.Models
{
    public class TripSearchResultDto
    {
        public int TripId { get; set; }
        public string CompanyName { get; set; }
        public string VehicleType { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public decimal Price { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public int Capacity { get; set; }
        public int AvailableSeats { get; set; }
    }
}
