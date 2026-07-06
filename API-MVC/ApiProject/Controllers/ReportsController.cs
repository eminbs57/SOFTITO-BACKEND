using ApiProject.Data;
using ApiProject.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReportsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetTripReport")]
        public async Task<ActionResult<IEnumerable<TripReportDto>>> GetTripReport()
        {
            // 5 adet JOIN islemi (Include ve Select ile)
            var reportData = await _context.Trips
                .Include(t => t.Driver)     // Join 1
                .Include(t => t.Vehicle)    // Join 2
                .Include(t => t.Route)      // Join 3
                    .ThenInclude(r => r!.StartDepo) // Join 4
                .Include(t => t.Route)
                    .ThenInclude(r => r!.EndDepo)   // Join 5
                .Select(t => new TripReportDto
                {
                    TripId = t.Id,
                    TripDate = t.TripDate,
                    DriverFullName = t.Driver!.FirstName + " " + t.Driver.LastName,
                    LicensePlate = t.Vehicle!.LicensePlate,
                    RouteName = t.Route!.RouteName,
                    StartDepoName = t.Route.StartDepo!.DepoName + " (" + t.Route.StartDepo.City + ")",
                    EndDepoName = t.Route.EndDepo!.DepoName + " (" + t.Route.EndDepo.City + ")",
                    Distance = t.Route.Distance
                })
                .ToListAsync();

            return Ok(reportData);
        }

        [HttpGet("GetDriverStats")]
        public async Task<ActionResult<IEnumerable<DriverStatDto>>> GetDriverStats()
        {
            var stats = await _context.Trips
                .Include(t => t.Driver)
                .GroupBy(t => new { t.DriverId, t.Driver!.FirstName, t.Driver.LastName })
                .Select(g => new DriverStatDto
                {
                    DriverFullName = g.Key.FirstName + " " + g.Key.LastName,
                    TotalTrips = g.Count()
                })
                .OrderByDescending(s => s.TotalTrips)
                .ToListAsync();

            return Ok(stats);
        }

        [HttpGet("GetRouteStats")]
        public async Task<ActionResult<IEnumerable<RouteStatDto>>> GetRouteStats()
        {
            var stats = await _context.Trips
                .Include(t => t.Route)
                .GroupBy(t => new { t.RouteId, t.Route!.RouteName })
                .Select(g => new RouteStatDto
                {
                    RouteName = g.Key.RouteName,
                    TotalTrips = g.Count(),
                    TotalDistance = g.Sum(t => t.Route!.Distance)
                })
                .OrderByDescending(s => s.TotalTrips)
                .ToListAsync();

            return Ok(stats);
        }

        [HttpGet("GetVehicleStats")]
        public async Task<ActionResult<IEnumerable<VehicleStatDto>>> GetVehicleStats()
        {
            var stats = await _context.Trips
                .Include(t => t.Vehicle)
                .GroupBy(t => new { t.VehicleId, t.Vehicle!.LicensePlate, t.Vehicle.Brand })
                .Select(g => new VehicleStatDto
                {
                    LicensePlate = g.Key.LicensePlate,
                    Brand = g.Key.Brand,
                    TotalTrips = g.Count()
                })
                .OrderByDescending(s => s.TotalTrips)
                .ToListAsync();

            return Ok(stats);
        }
    }
}
