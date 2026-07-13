using Microsoft.AspNetCore.Mvc;
using ObiletApp.Core.Entities;
using ObiletApp.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.Extensions.Caching.Memory;
using System;

namespace ObiletApp.API.Dapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyQueryController : ControllerBase
    {
        private readonly IDapperQueryService _dapperQueryService;
        private readonly IMemoryCache _cache;

        public CompanyQueryController(IDapperQueryService dapperQueryService, IMemoryCache cache)
        {
            _dapperQueryService = dapperQueryService;
            _cache = cache;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            const string cacheKey = "CompanyList";
            if (!_cache.TryGetValue(cacheKey, out IEnumerable<Company> companies))
            {
                string sql = "SELECT * FROM Companies";
                companies = await _dapperQueryService.QueryAsync<Company>(sql);

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(10));

                _cache.Set(cacheKey, companies, cacheOptions);
            }
            return Ok(companies);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            string sql = "SELECT * FROM Companies WHERE Id = @Id";
            var company = await _dapperQueryService.QueryFirstOrDefaultAsync<Company>(sql, new { Id = id });
            if (company == null) return NotFound();
            return Ok(company);
        }
    }
}
