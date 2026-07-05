using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace PetAdoptionORM.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class LogController : Controller
    {
        public IActionResult Index()
        {
            var logs = new List<string>();
            try
            {
                var logDirectory = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Logs");
                if (System.IO.Directory.Exists(logDirectory))
                {
                    // You can list all logs or just the latest. Let's read up to 100 lines from the latest log.
                    var latestLogFile = new System.IO.DirectoryInfo(logDirectory).GetFiles("*.txt")
                                                                     .OrderByDescending(f => f.LastWriteTime)
                                                                     .FirstOrDefault();
                    if (latestLogFile != null)
                    {
                        var allLines = System.IO.File.ReadLines(latestLogFile.FullName).ToList();
                        var lastLines = allLines.AsEnumerable().Reverse().Take(100).ToList();
                        foreach (var line in lastLines)
                        {
                            if (string.IsNullOrWhiteSpace(line)) continue;
                            
                            // Parse log message
                            int bracketIndex = line.IndexOf(']');
                            if (bracketIndex != -1 && line.Length > bracketIndex + 1)
                            {
                                string timePart = line.Substring(0, bracketIndex + 1); // Get full timestamp + level
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

            return View(logs);
        }
    }
}
