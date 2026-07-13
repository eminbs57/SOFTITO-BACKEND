using ObiletApp.Core.Common;
using ObiletApp.Core.Enums;

namespace ObiletApp.Core.Entities
{
    public class Ticket : BaseEntity
    {
        public int TripId { get; set; }
        public int PassengerId { get; set; }

        public string? UserId { get; set; }

        public string? SeatNumber { get; set; }
        public string? PNR { get; set; }
        public TicketStatus Status { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public string? UserName { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public string? PassengerFullName { get; set; }

        public Trip? Trip { get; set; }
        public Passenger? Passenger { get; set; }
        public Payment? Payment { get; set; }
    }
}