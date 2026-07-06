using System;

namespace MvcProject.Models
{
    public class TripReportDto
    {
        public int TripId { get; set; }
        public DateTime TripDate { get; set; }
        public string DriverFullName { get; set; } = string.Empty;
        public string LicensePlate { get; set; } = string.Empty;
        public string RouteName { get; set; } = string.Empty;
        public string StartDepoName { get; set; } = string.Empty;
        public string EndDepoName { get; set; } = string.Empty;
        public double Distance { get; set; }
    }
}
