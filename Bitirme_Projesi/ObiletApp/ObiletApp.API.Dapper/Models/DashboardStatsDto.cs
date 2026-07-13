namespace ObiletApp.API.Dapper.Models
{
    public class DashboardStatsDto
    {
        public int TotalTicketsSold { get; set; }
        public int ActiveTrips { get; set; }
        public decimal DailyRevenue { get; set; }
        public int TotalCompanies { get; set; }
        public System.Collections.Generic.List<RecentTicketDto> RecentTickets { get; set; } = new();
    }
}
