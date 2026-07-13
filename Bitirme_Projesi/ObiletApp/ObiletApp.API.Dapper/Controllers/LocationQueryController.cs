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
    public class LocationQueryController : ControllerBase
    {
        private readonly IDapperQueryService _queryService;
        private readonly IMemoryCache _cache;

        public LocationQueryController(IDapperQueryService queryService, IMemoryCache cache)
        {
            _queryService = queryService;
            _cache = cache;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            const string cacheKey = "LocationList";
            if (!_cache.TryGetValue(cacheKey, out IEnumerable<Location> data))
            {

                data = await _queryService.QueryAsync<Location>("SELECT * FROM Locations WHERE IsActive = 1");

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(10));

                _cache.Set(cacheKey, data, cacheOptions);
            }

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _queryService.QueryFirstOrDefaultAsync<Location>(
                "SELECT * FROM Locations WHERE Id = @Id AND IsActive = 1", new { Id = id });
            if (data == null) return NotFound();
            return Ok(data);
        }
    }
}