using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PetAdoptionORM.Data.Abstract;
using PetAdoptionORM.Model;
using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace PetAdoptionORM.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class PetController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly IWebHostEnvironment _env;

        public PetController(IUnitOfWork uow, IWebHostEnvironment env)
        {
            _uow = uow;
            _env = env;
        }

        public IActionResult Index()
        {
            var list = _uow.Pet.GetAll(includeProperties: "Breed,Breed.Species");
            return View(list);
        }

        public IActionResult Create()
        {
            ViewBag.BreedList = _uow.Breed.GetAll().Select(b => new SelectListItem
            {
                Text = b.BreedName,
                Value = b.Id.ToString()
            });
            return View();
        }

        [HttpPost]
        public IActionResult Create(Pet obj, IFormFile file)
        {
            ModelState.Remove("Breed");
            ModelState.Remove("Photo"); // Photo string formdan gelmediği için validasyondan çıkarıyoruz
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    string wwwRootPath = _env.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"img/pets");
                    
                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    obj.Photo = @"/img/pets/" + fileName;
                }
                
                _uow.Pet.Add(obj);
                _uow.Save();
                return RedirectToAction("Index");
            }
            ViewBag.BreedList = _uow.Breed.GetAll().Select(b => new SelectListItem
            {
                Text = b.BreedName,
                Value = b.Id.ToString()
            });
            return View(obj);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) return NotFound();
            var obj = _uow.Pet.GetFirstOrDefault(u => u.Id == id);
            if (obj == null) return NotFound();
            
            ViewBag.BreedList = _uow.Breed.GetAll().Select(b => new SelectListItem
            {
                Text = b.BreedName,
                Value = b.Id.ToString()
            });
            return View(obj);
        }

        [HttpPost]
        public IActionResult Edit(Pet obj, IFormFile file)
        {
            var dbObj = _uow.Pet.GetFirstOrDefault(u => u.Id == obj.Id);
            if (dbObj != null)
            {
                dbObj.Name = obj.Name;
                dbObj.Age = obj.Age;
                dbObj.HealthStatus = obj.HealthStatus;
                dbObj.BreedId = obj.BreedId;

                if (file != null)
                {
                    string wwwRootPath = _env.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"img/pets");
                    
                    // Delete old photo if exists
                    if (!string.IsNullOrEmpty(dbObj.Photo))
                    {
                        var oldPath = Path.Combine(wwwRootPath, dbObj.Photo.TrimStart('/'));
                        if (System.IO.File.Exists(oldPath))
                        {
                            System.IO.File.Delete(oldPath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    dbObj.Photo = @"/img/pets/" + fileName;
                }
                _uow.Save();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int? id)
        {
            var obj = _uow.Pet.GetFirstOrDefault(u => u.Id == id);
            if (obj != null)
            {
                if (!string.IsNullOrEmpty(obj.Photo))
                {
                    var oldPath = Path.Combine(_env.WebRootPath, obj.Photo.TrimStart('/'));
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }
                _uow.Pet.Remove(obj);
                _uow.Save();
            }
            return RedirectToAction("Index");
        }
    }
}
