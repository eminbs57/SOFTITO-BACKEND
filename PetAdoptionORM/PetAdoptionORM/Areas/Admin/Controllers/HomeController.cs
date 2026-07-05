using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetAdoptionORM.Data.Concrete;
using PetAdoptionORM.Model;
using System.Linq;
using System.Collections.Generic;

namespace PetAdoptionORM.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var model = new DashboardViewModel();

            // Card Totals
            model.TotalSpecies = _db.Species.Count();
            model.TotalBreeds = _db.Breeds.Count();
            model.TotalPets = _db.Pets.Count();
            
            // Dummy logic for Total Reports (using number of pets for now as an example, or joining)
            model.TotalReports = (from p in _db.Pets
                                  join b in _db.Breeds on p.BreedId equals b.Id
                                  select p).Count(); // Normally this would be a Reports table count

            // Chart Data: Number of pets by species
            var petsBySpecies = _db.Pets
                .GroupBy(p => p.Breed.Species.SpeciesName)
                .Select(g => new { SpeciesName = g.Key, Count = g.Count() })
                .ToList();

            foreach (var item in petsBySpecies)
            {
                model.ChartLabels.Add(item.SpeciesName);
                model.ChartData.Add(item.Count);
            }

            // Recent Activities (Latest 5 pets added)
            var recentPets = _db.Pets
                .OrderByDescending(p => p.Id)
                .Take(5)
                .Select(p => new ActivityItem
                {
                    Title = p.Name,
                    Description = p.Breed.BreedName + " ırkı sisteme eklendi.",
                    IconClass = "bi-emoji-smile",
                    IconColorClass = "text-primary",
                    TimeAgo = "Yeni"
                }).ToList();

            model.RecentActivities = recentPets;

            // Read Serilog Files
            var logs = new List<string>();
            try
            {
                var logDirectory = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Logs");
                if (System.IO.Directory.Exists(logDirectory))
                {
                    var latestLogFile = new System.IO.DirectoryInfo(logDirectory).GetFiles("*.txt")
                                                                     .OrderByDescending(f => f.LastWriteTime)
                                                                     .FirstOrDefault();
                    if (latestLogFile != null)
                    {
                        var allLines = System.IO.File.ReadLines(latestLogFile.FullName).ToList();
                        var lastLines = allLines.AsEnumerable().Reverse().Take(10).ToList();
                        foreach (var line in lastLines)
                        {
                            if (string.IsNullOrWhiteSpace(line)) continue;
                            
                            // Extract just the message and timestamp to hide technical IDs/Exceptions if any
                            int bracketIndex = line.IndexOf(']');
                            if (bracketIndex != -1 && line.Length > bracketIndex + 1)
                            {
                                string timePart = line.Substring(11, 8); // hh:mm:ss
                                string msgPart = line.Substring(bracketIndex + 1).Trim();
                                logs.Add($"{timePart} - {msgPart}");
                            }
                            else
                            {
                                logs.Add(line);
                            }
                        }
                    }
                }
            } catch { }
            model.SystemLogs = logs;

            return View(model);
        }
    }
}
