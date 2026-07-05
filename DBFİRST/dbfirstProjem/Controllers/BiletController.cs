using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using dbfirstProjem.Models;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace dbfirstProjem.Controllers
{
    [Authorize(Roles = "User")]
    public class BiletController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BiletController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult KoltukSec(int seansId)
        {
            var secilenSeans = _context.Seanslars
            .Include(s => s.Film)
            .Include(s => s.Salon)
            .FirstOrDefault(s => s.SeansId == seansId);

            if (secilenSeans == null) return NotFound("Seans bulunamadı.");

            ViewBag.FilmAdi = secilenSeans.Film?.Ad;
            ViewBag.SalonAdi = secilenSeans.Salon?.Ad;
            ViewBag.SeansId = secilenSeans.SeansId;

            var koltuklar = _context.Koltuklars.
            Where(s => s.SalonId == secilenSeans.SalonId)
            .ToList();

            var doluKoltukIdleri = _context.Biletlers
            .Where(s => s.SeansId == seansId)
            .Select(s => s.KoltukId)
            .ToList();

            ViewBag.DoluKoltuklar = doluKoltukIdleri;
            return View(koltuklar);
        }

        public IActionResult SatinAl(int seansId, int koltukId)
        {
            bool doluMu = _context.Biletlers.Any(b => b.SeansId == seansId && b.KoltukId == koltukId);

            if (doluMu)
            {
                TempData["Error"] = "Bu koltuk zaten satılmış!";
                return RedirectToAction("KoltukSec", new { seansId });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var seans = _context.Seanslars.Include(s => s.Film).FirstOrDefault(s => s.SeansId == seansId);
            decimal biletFiyat = seans?.Film?.Fiyat ?? 150.00m;

            var yeniBilet = new Biletler
            {
                SeansId = seansId,
                KoltukId = koltukId,
                Fiyat = biletFiyat,
                SatisTarihi = System.DateTime.Now,
                UserId = userId
            };
            _context.Biletlers.Add(yeniBilet);
            var koltuk = _context.Koltuklars.Find(koltukId);
            if (koltuk != null)
            {
                koltuk.DoluMu = true;
            }
            _context.SaveChanges();

            return RedirectToAction("BiletDetay", new { biletId = yeniBilet.BiletId });
        }

        public IActionResult BiletDetay(int biletId)
        {
            var bilet = _context.Biletlers
            .Include(b => b.Seans)
            .ThenInclude(s => s.Film)
            .Include(b => b.Seans)
            .ThenInclude(s => s.Salon)
            .Include(b => b.Koltuk)
            .FirstOrDefault(b => b.BiletId == biletId);
            if (bilet == null)
            {
                return NotFound("Bilet bulunamadı.");
            }

            return View(bilet);
        }

        public IActionResult Biletlerim()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var biletler = _context.Biletlers
                .Include(b => b.Seans).ThenInclude(s => s.Film)
                .Include(b => b.Seans).ThenInclude(s => s.Salon)
                .Include(b => b.Koltuk)
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.SatisTarihi)
                .ToList();

            return View(biletler);
        }
    }
}