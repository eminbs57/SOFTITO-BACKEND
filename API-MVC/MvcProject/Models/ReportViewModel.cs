using System.Collections.Generic;

namespace MvcProject.Models
{
    public class ReportViewModel
    {
        public List<TripReportDto> Trips { get; set; } = new List<TripReportDto>();
        public List<DriverStatDto> DriverStats { get; set; } = new List<DriverStatDto>();
        public List<RouteStatDto> RouteStats { get; set; } = new List<RouteStatDto>();
        public List<VehicleStatDto> VehicleStats { get; set; } = new List<VehicleStatDto>();
    }
}
