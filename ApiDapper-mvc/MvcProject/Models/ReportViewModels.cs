using System;
using System.Collections.Generic;

namespace MvcProject.Models
{
    public class RoomTypeRevenueDto
    {
        public string RoomTypeName { get; set; }
        public int TotalReservations { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    public class GuestStayDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int TotalStays { get; set; }
        public decimal TotalSpent { get; set; }
    }

    public class RoomAvailabilityDto
    {
        public string RoomTypeName { get; set; }
        public int AvailableCount { get; set; }
        public int OccupiedCount { get; set; }
    }

    public class UpcomingReservationDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RoomNumber { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
    }

    public class MonthlyRevenueDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int TotalBookings { get; set; }
        public decimal MonthlyRevenue { get; set; }
    }

    public class ReportsViewModel
    {
        public IEnumerable<RoomTypeRevenueDto> RoomTypeRevenues { get; set; }
        public IEnumerable<GuestStayDto> GuestStays { get; set; }
        public IEnumerable<RoomAvailabilityDto> RoomAvailabilities { get; set; }
        public IEnumerable<UpcomingReservationDto> UpcomingReservations { get; set; }
        public IEnumerable<MonthlyRevenueDto> MonthlyRevenues { get; set; }
    }
}
