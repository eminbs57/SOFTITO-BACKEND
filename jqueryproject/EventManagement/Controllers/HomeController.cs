using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using EventManagement.Models;
using EventManagement.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EventManagement.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ApplicationDbContext context, UserManager<AppUser> userManager, ILogger<HomeController> logger)
    {
        _context = context;
        _userManager = userManager;
        _logger = logger;
    }

    public IActionResult Index()
    {
        _logger.LogInformation("Kullanıcı anasayfaya (Index) giriş yaptı.");
        return View();
    }

    [Authorize]
    public IActionResult UserScreen()
    {
        _logger.LogInformation("Kullanıcı etkinlikler ekranına (UserScreen) giriş yaptı.");
        var events = _context.Events.ToList();
        return View(events);
    }

    [Authorize]
    public async Task<IActionResult> JoinEvent(int id)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login", "Account");

        // Include Attendees to check if the user has already joined
        var ev = _context.Events.Include(e => e.Attendees).FirstOrDefault(e => e.Id == id);
        if (ev != null)
        {
            if (!ev.Attendees.Any(u => u.Id == user.Id))
            {
                if (ev.Kota > 0)
                {
                    ev.Kota--;
                    ev.Attendees.Add(user);
                    _context.SaveChanges();
                }
                else
                {
                    TempData["ErrorMessage"] = "Üzgünüz, bu etkinlik için kontenjan dolmuştur.";
                    return RedirectToAction("UserScreen");
                }
            }
        }
        return RedirectToAction("MyEvents");
    }

    [Authorize]
    public async Task<IActionResult> MyEvents()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login", "Account");

        var events = _context.Events
            .Include(e => e.Attendees)
            .Where(e => e.Attendees.Any(u => u.Id == user.Id))
            .ToList();
            
        return View(events);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
