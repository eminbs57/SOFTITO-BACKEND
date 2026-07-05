using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PetAdoptionORM.Data.Concrete;
using PetAdoptionORM.Model;
using System.Linq;
using System.Collections.Generic;

namespace PetAdoptionORM.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ReportController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ReportController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index(int? reportType, string parameterValue)
        {
            // ViewBag for Dropdowns
            ViewBag.SpeciesList = new SelectList(_db.Species.ToList(), "Id", "SpeciesName");
            ViewBag.BreedList = new SelectList(_db.Breeds.ToList(), "Id", "BreedName");
            ViewBag.PetList = new SelectList(_db.Pets.ToList(), "Name", "Name"); // By Name
            
            var healthStatuses = _db.Pets
                .Select(p => p.HealthStatus.ToLower())
                .Distinct()
                .Select(h => new SelectListItem { Text = h, Value = h })
                .ToList();
            ViewBag.HealthStatusList = new SelectList(healthStatuses, "Value", "Text");

            var ageList = _db.Pets
                .Select(p => p.Age.ToString())
                .Distinct()
                .Select(a => new SelectListItem { Text = a + " Yaşında", Value = a })
                .ToList();
            ViewBag.AgeList = new SelectList(ageList, "Value", "Text");

            ViewBag.SelectedReport = reportType;
            List<ReportViewModel> reportData = new List<ReportViewModel>();

            if (reportType.HasValue && !string.IsNullOrEmpty(parameterValue))
            {
                switch (reportType.Value)
                {
                    case 1: // Türe Göre (Pets JOIN Breeds JOIN Species)
                        int speciesId = int.Parse(parameterValue);
                        reportData = (from p in _db.Pets
                                      join b in _db.Breeds on p.BreedId equals b.Id
                                      join s in _db.Species on b.SpeciesId equals s.Id
                                      where s.Id == speciesId
                                      select new ReportViewModel
                                      {
                                          PetName = p.Name, Age = p.Age, HealthStatus = p.HealthStatus,
                                          BreedName = b.BreedName, SpeciesName = s.SpeciesName
                                      }).ToList();
                        break;

                    case 2: // Irka Göre (Pets JOIN Breeds JOIN Species)
                        int breedId = int.Parse(parameterValue);
                        reportData = (from p in _db.Pets
                                      join b in _db.Breeds on p.BreedId equals b.Id
                                      join s in _db.Species on b.SpeciesId equals s.Id
                                      where b.Id == breedId
                                      select new ReportViewModel
                                      {
                                          PetName = p.Name, Age = p.Age, HealthStatus = p.HealthStatus,
                                          BreedName = b.BreedName, SpeciesName = s.SpeciesName
                                      }).ToList();
                        break;

                    case 3: // Sağlık Durumuna Göre (Pets JOIN Breeds JOIN Species)
                        reportData = (from p in _db.Pets
                                      join b in _db.Breeds on p.BreedId equals b.Id
                                      join s in _db.Species on b.SpeciesId equals s.Id
                                      where p.HealthStatus.ToLower() == parameterValue.ToLower()
                                      select new ReportViewModel
                                      {
                                          PetName = p.Name, Age = p.Age, HealthStatus = p.HealthStatus,
                                          BreedName = b.BreedName, SpeciesName = s.SpeciesName
                                      }).ToList();
                        break;

                    case 4: // Yaşa Göre (Pets JOIN Breeds JOIN Species)
                        int age = int.Parse(parameterValue);
                        reportData = (from p in _db.Pets
                                      join b in _db.Breeds on p.BreedId equals b.Id
                                      join s in _db.Species on b.SpeciesId equals s.Id
                                      where p.Age == age
                                      select new ReportViewModel
                                      {
                                          PetName = p.Name, Age = p.Age, HealthStatus = p.HealthStatus,
                                          BreedName = b.BreedName, SpeciesName = s.SpeciesName
                                      }).ToList();
                        break;

                    case 5: // Hayvan Adına Göre (Pets JOIN Breeds JOIN Species)
                        reportData = (from p in _db.Pets
                                      join b in _db.Breeds on p.BreedId equals b.Id
                                      join s in _db.Species on b.SpeciesId equals s.Id
                                      where p.Name == parameterValue
                                      select new ReportViewModel
                                      {
                                          PetName = p.Name, Age = p.Age, HealthStatus = p.HealthStatus,
                                          BreedName = b.BreedName, SpeciesName = s.SpeciesName
                                      }).ToList();
                        break;
                }
            }

            return View(reportData);
        }
    }
}
