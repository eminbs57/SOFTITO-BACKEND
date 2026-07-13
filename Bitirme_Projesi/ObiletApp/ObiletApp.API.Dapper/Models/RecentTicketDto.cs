namespace ObiletApp.API.Dapper.Models
{
    public class RecentTicketDto
    {
        public string PNR { get; set; }
        public string PassengerName { get; set; }
        public string RouteName { get; set; }
        public string Date { get; set; }
        public string Status { get; set; }
    }
}
