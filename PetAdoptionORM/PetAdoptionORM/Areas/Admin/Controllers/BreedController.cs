using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PetAdoptionORM.Data.Abstract;
using PetAdoptionORM.Model;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace PetAdoptionORM.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BreedController : Controller
    {
        private readonly IUnitOfWork _uow;
        public BreedController(IUnitOfWork uow) { _uow = uow; }

        public IActionResult Index()
        {
            var list = _uow.Breed.GetAll(includeProperties: "Species");
            return View(list);
        }

        public IActionResult Create()
        {
            ViewBag.SpeciesList = _uow.Species.GetAll().Select(s => new SelectListItem
            {
                Text = s.SpeciesName,
                Value = s.Id.ToString()
            });
            return View();
        }

        [HttpPost]
        public IActionResult Create(Breed obj)
        {
            ModelState.Remove("Species");
            if (ModelState.IsValid)
            {
                _uow.Breed.Add(obj);
                _uow.Save();
                return RedirectToAction("Index");
            }
            ViewBag.SpeciesList = _uow.Species.GetAll().Select(s => new SelectListItem
            {
                Text = s.SpeciesName,
                Value = s.Id.ToString()
            });
            return View(obj);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) return NotFound();
            var obj = _uow.Breed.GetFirstOrDefault(u => u.Id == id);
            if (obj == null) return NotFound();
            
            ViewBag.SpeciesList = _uow.Species.GetAll().Select(s => new SelectListItem
            {
                Text = s.SpeciesName,
                Value = s.Id.ToString()
            });
            return View(obj);
        }

        [HttpPost]
        public IActionResult Edit(Breed obj)
        {
            var dbObj = _uow.Breed.GetFirstOrDefault(u => u.Id == obj.Id);
            if (dbObj != null)
            {
                dbObj.BreedName = obj.BreedName;
                dbObj.SpeciesId = obj.SpeciesId;
                _uow.Save();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int? id)
        {
            var obj = _uow.Breed.GetFirstOrDefault(u => u.Id == id);
            if (obj != null)
            {
                _uow.Breed.Remove(obj);
                _uow.Save();
            }
            return RedirectToAction("Index");
        }
    }
}
