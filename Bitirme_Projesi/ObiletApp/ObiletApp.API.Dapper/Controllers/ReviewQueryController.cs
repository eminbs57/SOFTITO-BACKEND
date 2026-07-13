using Microsoft.AspNetCore.Mvc;
using ObiletApp.Core.Entities;
using ObiletApp.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ObiletApp.API.Dapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewQueryController : ControllerBase
    {
        private readonly IDapperQueryService _queryService;

        public ReviewQueryController(IDapperQueryService queryService)
        {
            _queryService = queryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var sql = @"
                SELECT r.*, u.FirstName + ' ' + u.LastName as UserName 
                FROM Reviews r 
                LEFT JOIN AspNetUsers u ON r.UserId = u.Id 
                WHERE r.IsActive = 1";
            var data = await _queryService.QueryAsync<Review>(sql);
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var sql = @"
                SELECT r.*, u.FirstName + ' ' + u.LastName as UserName 
                FROM Reviews r 
                LEFT JOIN AspNetUsers u ON r.UserId = u.Id 
                WHERE r.Id = @Id AND r.IsActive = 1";
            var data = await _queryService.QueryFirstOrDefaultAsync<Review>(sql, new { Id = id });
            if (data == null) return NotFound();
            return Ok(data);
        }
    }
}