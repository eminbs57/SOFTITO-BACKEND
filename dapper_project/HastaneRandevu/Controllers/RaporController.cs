using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using HastaneRandevu.Models;

namespace HastaneRandevu.Controllers
{
    [Authorize]
    public class RaporController : Controller
    {
        private readonly ILogger<RaporController> _logger;

        public RaporController(ILogger<RaporController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // Veritabanındaki RandevuRapor Stored Procedure'ünü çalıştır
            var raporListesi = Context.Listeleme<RandevuRaporModel>("RandevuRapor").ToList();
            
            _logger.LogInformation("Rapor sayfası görüntülendi. Toplam {Count} kayıt getirildi.", raporListesi.Count);

            return View(raporListesi);
        }
    }
}
