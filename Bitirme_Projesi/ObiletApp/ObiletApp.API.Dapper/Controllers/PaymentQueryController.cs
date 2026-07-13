using Microsoft.AspNetCore.Mvc;
using ObiletApp.Core.Entities;
using ObiletApp.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ObiletApp.API.Dapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentQueryController : ControllerBase
    {
        private readonly IDapperQueryService _queryService;

        public PaymentQueryController(IDapperQueryService queryService)
        {
            _queryService = queryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {

            var data = await _queryService.QueryAsync<Payment>("SELECT * FROM Payments WHERE IsActive = 1");
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _queryService.QueryFirstOrDefaultAsync<Payment>(
                "SELECT * FROM Payments WHERE Id = @Id AND IsActive = 1", new { Id = id });
            if (data == null) return NotFound();
            return Ok(data);
        }
    }
}