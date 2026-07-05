using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MvcProject.Models;
using MvcProject.Services;

using Microsoft.AspNetCore.Authorization;

namespace MvcProject.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ApiService<Room> _roomService;
    private readonly ApiService<Reservation> _reservationService;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ApiService<Room> roomService, ApiService<Reservation> reservationService, ILogger<HomeController> logger)
    {
        _roomService = roomService;
        _reservationService = reservationService;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        _logger.LogInformation("Dashboard sayfası görüntülendi. Oda ve Rezervasyon verileri çekiliyor...");
        var rooms = await _roomService.GetAllAsync();
        var reservations = await _reservationService.GetAllAsync();

        ViewBag.TotalRooms = rooms.Count();
        ViewBag.AvailableRooms = rooms.Count(r => r.IsAvailable);
        ViewBag.OccupiedRooms = rooms.Count(r => !r.IsAvailable);
        
        var today = System.DateTime.Today;
        ViewBag.CheckInsToday = reservations.Count(r => r.CheckInDate.Date == today);
        ViewBag.CheckOutsToday = reservations.Count(r => r.CheckOutDate.Date == today);

        return View();
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
