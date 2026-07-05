using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using MvcProject.Models;
using MvcProject.Services;

namespace MvcProject.Controllers
{
    public class RoomTypesController : Controller
    {
        private readonly ApiService<RoomType> _apiService;
        private readonly IMemoryCache _cache;

        public RoomTypesController(ApiService<RoomType> apiService, IMemoryCache cache)
        {
            _apiService = apiService;
            _cache = cache;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            string cacheKey = "roomTypesList";

            if (!_cache.TryGetValue(cacheKey, out IEnumerable<RoomType> data))
            {
                data = await _apiService.GetAllAsync();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(System.TimeSpan.FromMinutes(5)) 
                    .SetSlidingExpiration(System.TimeSpan.FromMinutes(2)); 

                _cache.Set(cacheKey, data, cacheOptions);
            }
            
            if (!string.IsNullOrEmpty(searchString))
            {
                data = data.Where(s => 
                    (s.Name != null && s.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase)) ||
                    (s.Description != null && s.Description.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                );
            }

            ViewData["CurrentFilter"] = searchString;
            return View(data);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoomType entity)
        {
            await _apiService.CreateAsync(entity);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var data = await _apiService.GetByIdAsync(id);
            if (data == null) return NotFound();
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, RoomType entity)
        {
            await _apiService.UpdateAsync(id, entity);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _apiService.DeleteAsync(id);
            }
            catch (System.Exception)
            {
                TempData["ErrorMessage"] = "Bu oda tipi kullanımda olduğu için silinemez. Lütfen önce bu tipe ait odaları silin veya değiştirin.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
