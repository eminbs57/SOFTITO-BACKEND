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
            // 5 Farklı Raporu Stored Procedure'lerden Çekme
            ViewBag.RaporPoliklinik = Context.Listeleme<RaporPoliklinikModel>("SP_RaporPoliklinik").ToList();
            ViewBag.RaporDoktor = Context.Listeleme<RaporDoktorModel>("SP_RaporDoktor").ToList();
            ViewBag.RaporAktifHasta = Context.Listeleme<RaporAktifHastaModel>("SP_RaporAktifHasta").ToList();
            ViewBag.RaporDurum = Context.Listeleme<RaporDurumModel>("SP_RaporDurum").ToList();
            ViewBag.RaporPopulerDoktor = Context.Listeleme<RaporPopulerDoktorModel>("SP_RaporPopulerDoktor").ToList();
            
            _logger.LogInformation("Gelişmiş 5'li Rapor sayfası görüntülendi.");

            return View();
        }
    }
}
