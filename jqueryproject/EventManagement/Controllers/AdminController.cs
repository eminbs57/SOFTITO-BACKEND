using Microsoft.AspNetCore.Mvc;
using EventManagement.Data;
using Microsoft.AspNetCore.Authorization;

namespace EventManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.TotalEvents = _context.Events.Count();
            ViewBag.TotalCategories = _context.Categories.Count();
            ViewBag.TotalUsers = _context.Users.Count();
            return View();
        }
    }
}
