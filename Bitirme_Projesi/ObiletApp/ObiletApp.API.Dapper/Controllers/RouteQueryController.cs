using Microsoft.AspNetCore.Mvc;
using ObiletApp.Core.Entities;
using ObiletApp.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ObiletApp.API.Dapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RouteQueryController : ControllerBase
    {
        private readonly IDapperQueryService _queryService;

        public RouteQueryController(IDapperQueryService queryService)
        {
            _queryService = queryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var sql = @"
                SELECT r.*, 
                       dl.TerminalName as DepartureLocationName, 
                       al.TerminalName as ArrivalLocationName 
                FROM Routes r
                LEFT JOIN Locations dl ON r.DepartureLocationId = dl.Id
                LEFT JOIN Locations al ON r.ArrivalLocationId = al.Id
                WHERE r.IsActive = 1";
            var data = await _queryService.QueryAsync<ObiletApp.Core.Entities.Route>(sql);
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var sql = @"
                SELECT r.*, 
                       dl.TerminalName as DepartureLocationName, 
                       al.TerminalName as ArrivalLocationName 
                FROM Routes r
                LEFT JOIN Locations dl ON r.DepartureLocationId = dl.Id
                LEFT JOIN Locations al ON r.ArrivalLocationId = al.Id
                WHERE r.Id = @Id AND r.IsActive = 1";
            var data = await _queryService.QueryFirstOrDefaultAsync<ObiletApp.Core.Entities.Route>(sql, new { Id = id });
            if (data == null) return NotFound();
            return Ok(data);
        }
    }
}