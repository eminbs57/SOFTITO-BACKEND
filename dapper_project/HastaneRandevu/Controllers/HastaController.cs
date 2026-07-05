using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using Dapper;
using HastaneRandevu.Models;

namespace HastaneRandevu.Controllers
{
    [Authorize]
    public class HastaController : Controller
    {
        private readonly ILogger<HastaController> _logger;
        private readonly IMemoryCache _cache;

        public HastaController(ILogger<HastaController> logger, IMemoryCache cache)
        {
            _logger = logger;
            _cache = cache;
        }

        public IActionResult Index(string searchTerm = "")
        {
            string cacheKey = "hastaList";

            if (!_cache.TryGetValue(cacheKey, out List<HastaModel> hastalar))
            {
                hastalar = Context.Listeleme<HastaModel>("HastaViewAll").ToList();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));

                _cache.Set(cacheKey, hastalar, cacheOptions);
            }

            _logger.LogInformation("Toplam {Count} adet hasta veri tabanından/cache'ten listelendi.", hastalar.Count);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                hastalar = hastalar.Where(h => h.AdSoyad.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            ViewBag.SearchTerm = searchTerm;
            return View(hastalar);
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
                param.Add("@HastaNo", id);
                return View(Context.Listeleme<HastaModel>("HastaViewByNo", param).FirstOrDefault());
            }
        }

        [HttpPost]
        public IActionResult EY(HastaModel hasta)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@HastaNo", hasta.HastaNo);
            param.Add("@AdSoyad", hasta.AdSoyad);
            param.Add("@Yas", hasta.Yas);
            param.Add("@Telefon", hasta.Telefon);
            param.Add("@Adres", hasta.Adres);
            Context.ExecuteReturn("HastaEY", param);
            
            _cache.Remove("hastaList"); // Invalidate cache
            
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id = 0)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@HastaNo", id);
            Context.ExecuteReturn("HastaSil", param);
            
            _cache.Remove("hastaList"); // Invalidate cache
            
            return RedirectToAction("Index");
        }
    }
}