using Microsoft.AspNetCore.Mvc;
using ObiletApp.API.Dapper.Models;
using ObiletApp.Core.Interfaces;
using System.Threading.Tasks;

namespace ObiletApp.API.Dapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportQueryController : ControllerBase
    {
        private readonly IDapperQueryService _queryService;

        public ReportQueryController(IDapperQueryService queryService)
        {
            _queryService = queryService;
        }

        [HttpGet("CompanyRevenue")]
        public async Task<IActionResult> GetCompanyRevenue()
        {
            var sql = @"
                SELECT 
                    c.Name as CompanyName, 
                    COUNT(t.Id) as TicketCount, 
                    ISNULL(SUM(tr.Price), 0) as TotalRevenue
                FROM Companies c
                LEFT JOIN Vehicles v ON c.Id = v.CompanyId
                LEFT JOIN Trips tr ON v.Id = tr.VehicleId AND tr.IsActive = 1
                LEFT JOIN Tickets t ON tr.Id = t.TripId AND t.Status = 1 AND t.IsActive = 1
                WHERE c.IsActive = 1
                GROUP BY c.Name
                ORDER BY TotalRevenue DESC";
            var data = await _queryService.QueryAsync<ReportCompanyRevenueDto>(sql);
            return Ok(data);
        }

        [HttpGet("RouteTraffic")]
        public async Task<IActionResult> GetRouteTraffic()
        {
            var sql = @"
                SELECT TOP 10
                    (locOrigin.CityName + ' - ' + locDest.CityName) as RouteName,
                    COUNT(t.Id) as TicketCount,
                    ISNULL(SUM(tr.Price), 0) as TotalRevenue
                FROM Routes r
                JOIN Locations locOrigin ON r.DepartureLocationId = locOrigin.Id
                JOIN Locations locDest ON r.ArrivalLocationId = locDest.Id
                JOIN Trips tr ON r.Id = tr.RouteId AND tr.IsActive = 1
                LEFT JOIN Tickets t ON tr.Id = t.TripId AND t.Status = 1 AND t.IsActive = 1
                WHERE r.IsActive = 1
                GROUP BY locOrigin.CityName, locDest.CityName
                ORDER BY TicketCount DESC";
            var data = await _queryService.QueryAsync<ReportRouteTrafficDto>(sql);
            return Ok(data);
        }

        [HttpGet("UserLoyalty")]
        public async Task<IActionResult> GetUserLoyalty()
        {
            var sql = @"
                SELECT TOP 10
                    (u.FirstName + ' ' + u.LastName) as UserName,
                    COUNT(t.Id) as TicketCount,
                    ISNULL(SUM(tr.Price), 0) as TotalSpent
                FROM AspNetUsers u
                JOIN Tickets t ON u.Id = t.UserId AND t.Status = 1 AND t.IsActive = 1
                JOIN Trips tr ON t.TripId = tr.Id
                GROUP BY u.FirstName, u.LastName
                ORDER BY TotalSpent DESC";
            var data = await _queryService.QueryAsync<ReportUserLoyaltyDto>(sql);
            return Ok(data);
        }

        [HttpGet("VehicleOccupancy")]
        public async Task<IActionResult> GetVehicleOccupancy()
        {
            var sql = @"
                SELECT 
                    v.Plate,
                    c.Name as CompanyName,
                    v.Capacity,
                    COUNT(t.Id) as SoldTickets,
                    CAST(IIF(v.Capacity = 0, 0, (COUNT(t.Id) * 100.0 / v.Capacity)) AS DECIMAL(5,2)) as OccupancyRate
                FROM Vehicles v
                JOIN Companies c ON v.CompanyId = c.Id
                LEFT JOIN Trips tr ON v.Id = tr.VehicleId AND tr.IsActive = 1 AND tr.DepartureTime > GETDATE()
                LEFT JOIN Tickets t ON tr.Id = t.TripId AND t.Status = 1 AND t.IsActive = 1
                WHERE v.IsActive = 1
                GROUP BY v.Plate, c.Name, v.Capacity
                ORDER BY OccupancyRate DESC";
            var data = await _queryService.QueryAsync<ReportVehicleOccupancyDto>(sql);
            return Ok(data);
        }

        [HttpGet("MonthlySales")]
        public async Task<IActionResult> GetMonthlySales()
        {
            var sql = @"
                SELECT 
                    YEAR(t.CreatedDate) as Year,
                    MONTH(t.CreatedDate) as Month,
                    DATENAME(month, t.CreatedDate) as MonthName,
                    COUNT(t.Id) as TicketCount,
                    ISNULL(SUM(tr.Price), 0) as TotalRevenue
                FROM Tickets t
                JOIN Trips tr ON t.TripId = tr.Id
                WHERE t.Status = 1 AND t.IsActive = 1
                GROUP BY YEAR(t.CreatedDate), MONTH(t.CreatedDate), DATENAME(month, t.CreatedDate)
                ORDER BY Year DESC, Month DESC";
            var data = await _queryService.QueryAsync<ReportMonthlySalesDto>(sql);
            return Ok(data);
        }

        [HttpGet("TicketStatus")]
        public async Task<IActionResult> GetTicketStatus()
        {
            var sql = @"
                SELECT 
                    CASE t.Status 
                        WHEN 1 THEN 'Onaylandı' 
                        WHEN 2 THEN 'İptal Edildi' 
                        WHEN 3 THEN 'Kullanıldı' 
                        ELSE 'Bekliyor' 
                    END as Status,
                    COUNT(t.Id) as TicketCount,
                    ISNULL(SUM(tr.Price), 0) as TotalAmount
                FROM Tickets t
                JOIN Trips tr ON t.TripId = tr.Id
                WHERE t.IsActive = 1
                GROUP BY t.Status";
            var data = await _queryService.QueryAsync<ReportTicketStatusDto>(sql);
            return Ok(data);
        }

        [HttpGet("UpcomingTrips")]
        public async Task<IActionResult> GetUpcomingTrips()
        {
            var sql = @"
                SELECT TOP 10
                    (locOrigin.CityName + ' - ' + locDest.CityName) as RouteName,
                    c.Name as CompanyName,
                    tr.DepartureTime,
                    v.Capacity,
                    COUNT(t.Id) as SoldTickets,
                    (v.Capacity - COUNT(t.Id)) as RemainingSeats
                FROM Trips tr
                JOIN Routes r ON tr.RouteId = r.Id
                JOIN Locations locOrigin ON r.DepartureLocationId = locOrigin.Id
                JOIN Locations locDest ON r.ArrivalLocationId = locDest.Id
                JOIN Vehicles v ON tr.VehicleId = v.Id
                JOIN Companies c ON v.CompanyId = c.Id
                LEFT JOIN Tickets t ON tr.Id = t.TripId AND t.Status = 1 AND t.IsActive = 1
                WHERE tr.DepartureTime > GETDATE() AND tr.IsActive = 1
                GROUP BY locOrigin.CityName, locDest.CityName, c.Name, tr.DepartureTime, v.Capacity
                ORDER BY tr.DepartureTime ASC";
            var data = await _queryService.QueryAsync<ReportUpcomingTripDto>(sql);
            return Ok(data);
        }

        [HttpGet("LocationTraffic")]
        public async Task<IActionResult> GetLocationTraffic()
        {
            var sql = @"
                SELECT 
                    l.CityName,
                    ISNULL(SUM(CASE WHEN r.DepartureLocationId = l.Id THEN 1 ELSE 0 END), 0) as DepartureCount,
                    ISNULL(SUM(CASE WHEN r.ArrivalLocationId = l.Id THEN 1 ELSE 0 END), 0) as ArrivalCount,
                    COUNT(r.Id) as TotalTraffic
                FROM Locations l
                LEFT JOIN Routes r ON (r.DepartureLocationId = l.Id OR r.ArrivalLocationId = l.Id) AND r.IsActive = 1
                WHERE l.IsActive = 1
                GROUP BY l.CityName
                ORDER BY TotalTraffic DESC";
            var data = await _queryService.QueryAsync<ReportLocationTrafficDto>(sql);
            return Ok(data);
        }
    }
}
