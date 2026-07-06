using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using Dapper;
using HastaneRandevu.Models;

namespace HastaneRandevu.Controllers
{
    [Authorize]
    public class DoktorController : Controller
    {
        private readonly ILogger<DoktorController> _logger;
        private readonly IMemoryCache _cache;

        public DoktorController(ILogger<DoktorController> logger, IMemoryCache cache)
        {
            _logger = logger;
            _cache = cache;
        }

        public IActionResult Index(string searchTerm = "")
        {
            string cacheKey = "doktorList";

            if (!_cache.TryGetValue(cacheKey, out List<DoktorModel> doktorlar))
            {
                doktorlar = Context.Listeleme<DoktorModel>("DoktorViewAll").ToList();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));

                _cache.Set(cacheKey, doktorlar, cacheOptions);
            }

            _logger.LogInformation("Toplam {Count} adet doktor veri tabanından/cache'ten listelendi.", doktorlar.Count);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                doktorlar = doktorlar.Where(d => d.AdSoyad.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) || 
                                                 (d.Unvan != null && d.Unvan.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))).ToList();
            }

            ViewBag.SearchTerm = searchTerm;
            ViewBag.Poliklinikler = Context.Listeleme<PoliklinikModel>("PoliklinikViewAll");
            return View(doktorlar);
        }

        public IActionResult EY(int id = 0)
        {
            // Dropdown için poliklinik listesini View'a taşıyoruz
            ViewBag.Poliklinikler = Context.Listeleme<PoliklinikModel>("PoliklinikViewAll");

            if (id == 0)
            {
                return View();
            }
            else
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@DoktorNo", id);
                return View(Context.Listeleme<DoktorModel>("DoktorViewByNo", param).FirstOrDefault());
            }
        }

        [HttpPost]
        public IActionResult EY(DoktorModel doktor)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@DoktorNo", doktor.DoktorNo);
            param.Add("@AdSoyad", doktor.AdSoyad);
            param.Add("@Unvan", doktor.Unvan);
            param.Add("@PoliklinikNo", doktor.PoliklinikNo);
            Context.ExecuteReturn("DoktorEY", param);
            
            _cache.Remove("doktorList"); // Invalidate cache
            
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id = 0)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@DoktorNo", id);
            Context.ExecuteReturn("DoktorSil", param);
            
            _cache.Remove("doktorList"); // Invalidate cache
            
            return RedirectToAction("Index");
        }
    }
}