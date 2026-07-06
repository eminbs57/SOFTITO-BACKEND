namespace ApiProject.DTOs
{
    public class RouteStatDto
    {
        public string RouteName { get; set; } = string.Empty;
        public int TotalTrips { get; set; }
        public double TotalDistance { get; set; }
    }
}
