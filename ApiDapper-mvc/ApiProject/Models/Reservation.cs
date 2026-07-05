using System;

namespace ApiProject.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public int GuestId { get; set; }
        public int RoomId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string FirstName { get; set; } // For ViewAll
        public string LastName { get; set; } // For ViewAll
        public string RoomNumber { get; set; } // For ViewAll
    }
}
