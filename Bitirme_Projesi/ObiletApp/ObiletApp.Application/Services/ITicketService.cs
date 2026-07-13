using ObiletApp.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ObiletApp.Application.Services
{
    public interface ITicketService
    {
        Task<Ticket> BuyTicketAsync(Ticket ticket);
        Task<IEnumerable<Ticket>> GetUserTicketsFastAsync(string userId);
    }
}
