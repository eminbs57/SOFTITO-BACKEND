using Microsoft.AspNetCore.Mvc;
using ObiletApp.Core.Entities;
using ObiletApp.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ObiletApp.API.Dapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripQueryController : ControllerBase
    {
        private readonly IDapperQueryService _queryService;

        public TripQueryController(IDapperQueryService queryService)
        {
            _queryService = queryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            string sql = @"
                SELECT 
                    t.Id, 
                    CONCAT(l1.CityName, ' - ', l2.CityName) as RouteName,
                    c.Name as CompanyName,
                    v.Plate as VehiclePlate,
                    t.DepartureTime, 
                    t.ArrivalTime, 
                    t.Price
                FROM Trips t
                JOIN Routes r ON t.RouteId = r.Id
                JOIN Vehicles v ON t.VehicleId = v.Id
                JOIN Companies c ON v.CompanyId = c.Id
                JOIN Locations l1 ON r.DepartureLocationId = l1.Id
                JOIN Locations l2 ON r.ArrivalLocationId = l2.Id
                WHERE t.IsActive = 1
                ORDER BY t.DepartureTime DESC";

            var data = await _queryService.QueryAsync<ObiletApp.API.Dapper.Models.TripAdminListDto>(sql);
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _queryService.QueryFirstOrDefaultAsync<Trip>(
                "SELECT * FROM Trips WHERE Id = @Id AND IsActive = 1", new { Id = id });
            if (data == null) return NotFound();
            return Ok(data);
        }

        [HttpGet("Search")]
        public async Task<IActionResult> Search([FromQuery] int originId, [FromQuery] int destinationId, [FromQuery] string date)
        {
            string sql = @"
                SELECT 
                    t.Id as TripId, 
                    c.Name as CompanyName, 
                    CASE v.Type 
                        WHEN 1 THEN 'Otobüs 2+1' 
                        WHEN 2 THEN 'Otobüs 2+2' 
                        WHEN 3 THEN 'Uçak' 
                        ELSE 'Standart' END as VehicleType,
                    t.DepartureTime, 
                    t.ArrivalTime, 
                    t.Price,
                    l1.CityName as Origin, 
                    l2.CityName as Destination,
                    v.Capacity,
                    (v.Capacity - (SELECT COUNT(*) FROM Tickets tic WHERE tic.TripId = t.Id AND tic.Status IN (1, 3))) as AvailableSeats
                FROM Trips t
                JOIN Routes r ON t.RouteId = r.Id
                JOIN Vehicles v ON t.VehicleId = v.Id
                JOIN Companies c ON v.CompanyId = c.Id
                JOIN Locations l1 ON r.DepartureLocationId = l1.Id
                JOIN Locations l2 ON r.ArrivalLocationId = l2.Id
                WHERE l1.Id = @OriginId AND l2.Id = @DestinationId AND CAST(t.DepartureTime AS DATE) = CAST(@Date AS DATE) AND t.IsActive = 1
                ORDER BY t.DepartureTime ASC";

            var data = await _queryService.QueryAsync<ObiletApp.API.Dapper.Models.TripSearchResultDto>(sql, new { OriginId = originId, DestinationId = destinationId, Date = date });
            return Ok(data);
        }

        [HttpGet("{id}/OccupiedSeats")]
        public async Task<IActionResult> GetOccupiedSeats(int id)
        {

            string sql = "SELECT SeatNumber FROM Tickets WHERE TripId = @TripId AND Status IN (1, 3)";
            var seats = await _queryService.QueryAsync<string>(sql, new { TripId = id });
            return Ok(seats);
        }
    }
}