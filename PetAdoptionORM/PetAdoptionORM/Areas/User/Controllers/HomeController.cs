using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetAdoptionORM.Data.Concrete;
using System.Linq;
using System.Threading.Tasks;

namespace PetAdoptionORM.Areas.User.Controllers
{
    [Area("User")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext db, ILogger<HomeController> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var pets = await _db.Pets
                .Include(p => p.Breed)
                .ThenInclude(b => b.Species)
                .OrderByDescending(p => p.Id)
                .ToListAsync();

            _logger.LogInformation("Kullanıcı anasayfasında {Count} adet sahiplendirilmeyi bekleyen hayvan listelendi.", pets.Count);

            return View(pets);
        }
    }
}
