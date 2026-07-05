#!/bin/bash
PROJECT_PATH="/Users/muhammeteminbas/Desktop/PetAdoptionORM/PetAdoptionORM"

# --- Admin Species Controller ---
cat << 'EOF' > "$PROJECT_PATH/Areas/Admin/Controllers/SpeciesController.cs"
using Microsoft.AspNetCore.Mvc;
using PetAdoptionORM.Data.Abstract;
using PetAdoptionORM.Model;

namespace PetAdoptionORM.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SpeciesController : Controller
    {
        private readonly IUnitOfWork _uow;
        public SpeciesController(IUnitOfWork uow) { _uow = uow; }

        public IActionResult Index()
        {
            var list = _uow.Species.GetAll();
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
    }
}
EOF

# --- Admin Breed Controller ---
cat << 'EOF' > "$PROJECT_PATH/Areas/Admin/Controllers/BreedController.cs"
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PetAdoptionORM.Data.Abstract;
using PetAdoptionORM.Model;
using System.Linq;

namespace PetAdoptionORM.Areas.Admin.Controllers
{
    [Area("Admin")]
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
            if (ModelState.IsValid)
            {
                _uow.Breed.Add(obj);
                _uow.Save();
                return RedirectToAction("Index");
            }
            return View(obj);
        }
    }
}
EOF

# --- Admin Pet Controller ---
cat << 'EOF' > "$PROJECT_PATH/Areas/Admin/Controllers/PetController.cs"
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PetAdoptionORM.Data.Abstract;
using PetAdoptionORM.Model;
using System;
using System.IO;
using System.Linq;

namespace PetAdoptionORM.Areas.Admin.Controllers
{
    [Area("Admin")]
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
            var list = _uow.Pet.GetAll(includeProperties: "Breed");
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
            return View(obj);
        }
    }
}
EOF
echo "Controllers Generated"
