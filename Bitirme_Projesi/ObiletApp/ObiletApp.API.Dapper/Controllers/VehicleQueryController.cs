using Microsoft.AspNetCore.Mvc;
using System.Linq;
using ObiletApp.Core.Entities;
using ObiletApp.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ObiletApp.API.Dapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleQueryController : ControllerBase
    {
        private readonly IDapperQueryService _queryService;

        public VehicleQueryController(IDapperQueryService queryService)
        {
            _queryService = queryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var sql = @"
                SELECT v.*, c.* 
                FROM Vehicles v 
                INNER JOIN Companies c ON v.CompanyId = c.Id 
                WHERE v.IsActive = 1";

            var data = await _queryService.QueryAsync<Vehicle, Company, Vehicle>(
                sql,
                (vehicle, company) => 
                {
                    vehicle.Company = company;
                    return vehicle;
                },
                splitOn: "Id"
            );
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var sql = @"
                SELECT v.*, c.* 
                FROM Vehicles v 
                INNER JOIN Companies c ON v.CompanyId = c.Id 
                WHERE v.Id = @Id AND v.IsActive = 1";

            var data = await _queryService.QueryAsync<Vehicle, Company, Vehicle>(
                sql,
                (vehicle, company) => 
                {
                    vehicle.Company = company;
                    return vehicle;
                },
                new { Id = id },
                splitOn: "Id"
            );

            var result = data.FirstOrDefault();
            if (result == null) return NotFound();
            return Ok(result);
        }
    }
}