using System;

namespace ObiletApp.Web.Models
{
    public class ReportCompanyRevenueDto
    {
        public string CompanyName { get; set; } = string.Empty;
        public int TicketCount { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    public class ReportRouteTrafficDto
    {
        public string RouteName { get; set; } = string.Empty;
        public int TicketCount { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    public class ReportUserLoyaltyDto
    {
        public string UserName { get; set; } = string.Empty;
        public int TicketCount { get; set; }
        public decimal TotalSpent { get; set; }
    }

    public class ReportVehicleOccupancyDto
    {
        public string Plate { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public int SoldTickets { get; set; }
        public decimal OccupancyRate { get; set; }
    }

    public class ReportMonthlySalesDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public string MonthName { get; set; } = string.Empty;
        public int TicketCount { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    public class ReportTicketStatusDto
    {
        public string Status { get; set; } = string.Empty;
        public int TicketCount { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class ReportUpcomingTripDto
    {
        public string RouteName { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public DateTime DepartureTime { get; set; }
        public int Capacity { get; set; }
        public int SoldTickets { get; set; }
        public int RemainingSeats { get; set; }
    }

    public class ReportLocationTrafficDto
    {
        public string CityName { get; set; } = string.Empty;
        public int DepartureCount { get; set; }
        public int ArrivalCount { get; set; }
        public int TotalTraffic { get; set; }
    }

    public class ReportDashboardViewModel
    {
        public List<ReportCompanyRevenueDto> CompanyRevenues { get; set; } = new();
        public List<ReportRouteTrafficDto> RouteTraffic { get; set; } = new();
        public List<ReportUserLoyaltyDto> UserLoyalty { get; set; } = new();
        public List<ReportVehicleOccupancyDto> VehicleOccupancy { get; set; } = new();
        public List<ReportMonthlySalesDto> MonthlySales { get; set; } = new();
        public List<ReportTicketStatusDto> TicketStatus { get; set; } = new();
        public List<ReportUpcomingTripDto> UpcomingTrips { get; set; } = new();
        public List<ReportLocationTrafficDto> LocationTraffic { get; set; } = new();
    }
}
