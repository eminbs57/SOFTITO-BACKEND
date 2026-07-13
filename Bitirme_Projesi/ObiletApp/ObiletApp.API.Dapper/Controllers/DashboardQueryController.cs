using Microsoft.AspNetCore.Mvc;
using ObiletApp.Core.Interfaces;
using ObiletApp.API.Dapper.Models;
using System.Threading.Tasks;
using System;

namespace ObiletApp.API.Dapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardQueryController : ControllerBase
    {
        private readonly IDapperQueryService _queryService;

        public DashboardQueryController(IDapperQueryService queryService)
        {
            _queryService = queryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetStats()
        {
            var stats = new DashboardStatsDto();

            string sqlRevenue = "SELECT ISNULL(SUM(tr.Price), 0) FROM Tickets t JOIN Trips tr ON t.TripId = tr.Id WHERE t.Status IN (1, 3)";
            stats.DailyRevenue = await _queryService.QueryFirstOrDefaultAsync<decimal?>(sqlRevenue) ?? 0;

            string sqlTickets = "SELECT COUNT(*) FROM Tickets";
            stats.TotalTicketsSold = await _queryService.QueryFirstOrDefaultAsync<int>(sqlTickets);

            string sqlTrips = "SELECT COUNT(*) FROM Trips";
            stats.ActiveTrips = await _queryService.QueryFirstOrDefaultAsync<int>(sqlTrips);

            string sqlCompanies = "SELECT COUNT(*) FROM Companies";
            stats.TotalCompanies = await _queryService.QueryFirstOrDefaultAsync<int>(sqlCompanies);

            string sqlRecentTickets = @"
                SELECT TOP 5 
                    t.PNR,
                    ISNULL(t.UserId, p.FirstName + ' ' + p.LastName) as PassengerName,
                    (locOrigin.CityName + ' - ' + locDest.CityName) as RouteName,
                    FORMAT(tr.DepartureTime, 'dd.MM.yyyy HH:mm') as Date,
                    CASE t.Status 
                        WHEN 1 THEN 'Onaylandı' 
                        WHEN 2 THEN 'İptal Edildi' 
                        WHEN 3 THEN 'Kullanıldı'
                        ELSE 'Bekliyor' 
                    END as Status
                FROM Tickets t
                LEFT JOIN Passengers p ON t.PassengerId = p.Id
                LEFT JOIN Trips tr ON t.TripId = tr.Id
                LEFT JOIN Routes r ON tr.RouteId = r.Id
                LEFT JOIN Locations locOrigin ON r.DepartureLocationId = locOrigin.Id
                LEFT JOIN Locations locDest ON r.ArrivalLocationId = locDest.Id
                ORDER BY t.CreatedDate DESC";

            var recentTickets = await _queryService.QueryAsync<RecentTicketDto>(sqlRecentTickets);
            if (recentTickets != null)
            {
                stats.RecentTickets.AddRange(recentTickets);
            }

            return Ok(stats);
        }
    }
}
