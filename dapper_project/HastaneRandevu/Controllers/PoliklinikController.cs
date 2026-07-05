using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using Dapper;
using HastaneRandevu.Models;

namespace HastaneRandevu.Controllers
{
    [Authorize]
    public class PoliklinikController : Controller
    {
        private readonly ILogger<PoliklinikController> _logger;
        private readonly IMemoryCache _cache;

        public PoliklinikController(ILogger<PoliklinikController> logger, IMemoryCache cache)
        {
            _logger = logger;
            _cache = cache;
        }

        public IActionResult Index(string searchTerm = "")
        {
            string cacheKey = "poliklinikList";

            if (!_cache.TryGetValue(cacheKey, out List<PoliklinikModel> poliklinikler))
            {
                poliklinikler = Context.Listeleme<PoliklinikModel>("PoliklinikViewAll").ToList();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));

                _cache.Set(cacheKey, poliklinikler, cacheOptions);
            }

            _logger.LogInformation("Toplam {Count} adet poliklinik veri tabanından/cache'ten listelendi.", poliklinikler.Count);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                poliklinikler = poliklinikler.Where(p => p.PoliklinikAdi.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            ViewBag.SearchTerm = searchTerm;
            return View(poliklinikler);
        }

        public IActionResult EY(int id = 0)
        {
            if (id == 0)
            {
                return View();
            }
            else
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@PoliklinikNo", id);
                return View(Context.Listeleme<PoliklinikModel>("PoliklinikViewByNo", param).FirstOrDefault());
            }
        }

        [HttpPost]
        public IActionResult EY(PoliklinikModel poliklinik)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@PoliklinikNo", poliklinik.PoliklinikNo);
            param.Add("@PoliklinikAdi", poliklinik.PoliklinikAdi);
            Context.ExecuteReturn("PoliklinikEY", param);
            
            _cache.Remove("poliklinikList"); // Invalidate cache
            
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id = 0)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@PoliklinikNo", id);
            Context.ExecuteReturn("PoliklinikSil", param);
            
            _cache.Remove("poliklinikList"); // Invalidate cache
            
            return RedirectToAction("Index");
        }
    }
}