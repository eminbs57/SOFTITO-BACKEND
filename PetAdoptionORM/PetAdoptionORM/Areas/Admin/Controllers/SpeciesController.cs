using Microsoft.AspNetCore.Mvc;
using PetAdoptionORM.Data.Abstract;
using PetAdoptionORM.Model;
using Microsoft.AspNetCore.Authorization;

namespace PetAdoptionORM.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SpeciesController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<SpeciesController> _logger;

        public SpeciesController(IUnitOfWork uow, ILogger<SpeciesController> logger) 
        { 
            _uow = uow; 
            _logger = logger;
        }

        public IActionResult Index()
        {
            var list = _uow.Species.GetAll();
            _logger.LogInformation("Toplam {Count} adet TÜR veritabanından listelendi.", list.Count());
            return View(list);
        }

        public IActionResult Create() { return View(); }

        [HttpPost]
        public IActionResult Create(Species obj)
        {
            if (ModelState.IsValid)
            {
                _uow.Species.Add(obj);
                _uow.Save();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) return NotFound();
            var obj = _uow.Species.GetFirstOrDefault(u => u.Id == id);
            if (obj == null) return NotFound();
            return View(obj);
        }

        [HttpPost]
        public IActionResult Edit(Species obj)
        {
            var dbObj = _uow.Species.GetFirstOrDefault(u => u.Id == obj.Id);
            if (dbObj != null)
            {
                dbObj.SpeciesName = obj.SpeciesName;
                _uow.Save();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int? id)
        {
            var obj = _uow.Species.GetFirstOrDefault(u => u.Id == id);
            if (obj != null)
            {
                _uow.Species.Remove(obj);
                _uow.Save();
            }
            return RedirectToAction("Index");
        }
    }
}
