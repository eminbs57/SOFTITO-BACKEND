using Microsoft.AspNetCore.Mvc;
using ObiletApp.Core.Entities;
using ObiletApp.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ObiletApp.API.Dapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PassengerQueryController : ControllerBase
    {
        private readonly IDapperQueryService _queryService;

        public PassengerQueryController(IDapperQueryService queryService)
        {
            _queryService = queryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {

            var data = await _queryService.QueryAsync<Passenger>("SELECT * FROM Passengers WHERE IsActive = 1");
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _queryService.QueryFirstOrDefaultAsync<Passenger>(
                "SELECT * FROM Passengers WHERE Id = @Id AND IsActive = 1", new { Id = id });
            if (data == null) return NotFound();
            return Ok(data);
        }
    }
}