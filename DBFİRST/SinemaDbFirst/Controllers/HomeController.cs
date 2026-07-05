using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using dbfirstProjem.Models;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace dbfirstProjem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [Authorize(Roles = "User")]
        public IActionResult KullaniciEkrani()
        {
            var filmler = _context.Filmlers.ToList();
            return View(filmler);
        }

        public IActionResult SeansSec(int filmId)
        {
            var secilenFilm = _context.Filmlers.FirstOrDefault(f => f.FilmId == filmId);
            ViewBag.FilmAdi = secilenFilm?.Ad;


            var seanslar = _context.Seanslars
                .Include(s => s.Salon)
                .Where(s => s.FilmId == filmId)
                .ToList();

            return View(seanslar);
        }
    }
}