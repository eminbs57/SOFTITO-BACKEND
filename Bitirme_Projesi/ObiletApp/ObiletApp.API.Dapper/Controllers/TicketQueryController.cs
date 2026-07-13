using Microsoft.AspNetCore.Mvc;
using ObiletApp.Core.Entities;
using ObiletApp.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ObiletApp.API.Dapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketQueryController : ControllerBase
    {
        private readonly IDapperQueryService _queryService;

        public TicketQueryController(IDapperQueryService queryService)
        {
            _queryService = queryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var sql = @"
                SELECT t.*, 
                       (u.FirstName + ' ' + u.LastName) as UserName,
                       (p.FirstName + ' ' + p.LastName) as PassengerFullName
                FROM Tickets t
                LEFT JOIN AspNetUsers u ON t.UserId = u.Id
                LEFT JOIN Passengers p ON t.PassengerId = p.Id
                WHERE t.IsActive = 1";
            var data = await _queryService.QueryAsync<Ticket>(sql);
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var sql = @"
                SELECT t.*, 
                       (u.FirstName + ' ' + u.LastName) as UserName,
                       (p.FirstName + ' ' + p.LastName) as PassengerFullName
                FROM Tickets t
                LEFT JOIN AspNetUsers u ON t.UserId = u.Id
                LEFT JOIN Passengers p ON t.PassengerId = p.Id
                WHERE t.Id = @Id AND t.IsActive = 1";
            var data = await _queryService.QueryFirstOrDefaultAsync<Ticket>(sql, new { Id = id });
            if (data == null) return NotFound();
            return Ok(data);
        }

        [HttpGet("Pnr/{pnr}")]
        public async Task<IActionResult> GetByPnr(string pnr, [FromQuery] string name)
        {
            var sql = @"
                SELECT t.*, 
                       ISNULL(t.UserId, p.FirstName + ' ' + p.LastName) as PassengerFullName,
                       locOrigin.CityName + ' - ' + locDest.CityName as UserName, /* Using UserName field for RouteName to save DTO mapping effort for now */
                       tr.DepartureTime as CreatedDate, /* Using CreatedDate for DepartureTime temporarily to simplify front-end mapping */
                       c.Name as PNR /* Temp use for CompanyName */
                FROM Tickets t
                LEFT JOIN Passengers p ON t.PassengerId = p.Id
                LEFT JOIN Trips tr ON t.TripId = tr.Id
                LEFT JOIN Routes r ON tr.RouteId = r.Id
                LEFT JOIN Locations locOrigin ON r.DepartureLocationId = locOrigin.Id
                LEFT JOIN Locations locDest ON r.ArrivalLocationId = locDest.Id
                LEFT JOIN Vehicles v ON tr.VehicleId = v.Id
                LEFT JOIN Companies c ON v.CompanyId = c.Id
                WHERE t.PNR = @Pnr AND t.IsActive = 1";

            var ticket = await _queryService.QueryFirstOrDefaultAsync<Ticket>(sql, new { Pnr = pnr });

            if (ticket == null) return NotFound(new { message = "Bilet bulunamadı." });

            if (!string.IsNullOrEmpty(name))
            {
                var storedName = ticket.PassengerFullName ?? "";
                if (!storedName.Equals(name, System.StringComparison.OrdinalIgnoreCase))
                {
                    return BadRequest(new { message = "Girilen isim bilet sahibi ile eşleşmiyor." });
                }
            }

            var companyName = ticket.PNR;
            ticket.PNR = pnr;

            return Ok(new {
                Id = ticket.Id,
                PNR = ticket.PNR,
                PassengerName = ticket.PassengerFullName,
                RouteName = ticket.UserName,
                DepartureTime = ticket.CreatedDate,
                SeatNumber = ticket.SeatNumber,
                Status = ticket.Status,
                CompanyName = companyName,
                TripId = ticket.TripId
            });
        }
    }
}