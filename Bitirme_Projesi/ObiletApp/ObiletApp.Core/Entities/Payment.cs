using ObiletApp.Core.Common;
using ObiletApp.Core.Enums;

namespace ObiletApp.Core.Entities
{
    public class Payment : BaseEntity
    {
        public int TicketId { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod Method { get; set; }
        public bool IsSuccessful { get; set; }

        public Ticket? Ticket { get; set; }
    }
}
