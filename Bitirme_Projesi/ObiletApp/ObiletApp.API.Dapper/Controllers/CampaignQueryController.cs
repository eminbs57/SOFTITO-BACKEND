using Microsoft.AspNetCore.Mvc;
using ObiletApp.Core.Entities;
using ObiletApp.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ObiletApp.API.Dapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignQueryController : ControllerBase
    {
        private readonly IDapperQueryService _queryService;

        public CampaignQueryController(IDapperQueryService queryService)
        {
            _queryService = queryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var sql = @"
                SELECT c.*, co.Name as CompanyName
                FROM Campaigns c
                LEFT JOIN Companies co ON c.CompanyId = co.Id
                WHERE c.IsActive = 1";
            var data = await _queryService.QueryAsync<Campaign>(sql);
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var sql = @"
                SELECT c.*, co.Name as CompanyName
                FROM Campaigns c
                LEFT JOIN Companies co ON c.CompanyId = co.Id
                WHERE c.Id = @Id AND c.IsActive = 1";
            var data = await _queryService.QueryFirstOrDefaultAsync<Campaign>(sql, new { Id = id });
            if (data == null) return NotFound();
            return Ok(data);
        }
    }
}