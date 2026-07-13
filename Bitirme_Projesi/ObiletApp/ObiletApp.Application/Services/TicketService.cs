using ObiletApp.Application.Services;
using ObiletApp.Core.Entities;
using ObiletApp.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ObiletApp.Application.Services
{
    public class TicketService : ITicketService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDapperQueryService _dapperService;

        public TicketService(IUnitOfWork unitOfWork, IDapperQueryService dapperService)
        {
            _unitOfWork = unitOfWork;
            _dapperService = dapperService;
        }

        public async Task<Ticket> BuyTicketAsync(Ticket ticket)
        {
            var ticketRepo = _unitOfWork.Repository<Ticket>();

            await ticketRepo.AddAsync(ticket);
            await _unitOfWork.SaveChangesAsync(); 

            return ticket;
        }

        public async Task<IEnumerable<Ticket>> GetUserTicketsFastAsync(string userId)
        {

            string sql = @"
                SELECT t.*, p.FirstName, p.LastName 
                FROM Tickets t
                INNER JOIN Passengers p ON t.PassengerId = p.Id
                WHERE t.UserId = @UserId AND t.IsActive = 1";

            return await _dapperService.QueryAsync<Ticket>(sql, new { UserId = userId });
        }
    }
}
