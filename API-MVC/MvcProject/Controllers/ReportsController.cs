using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcProject.Data;
using MvcProject.Models;
using MvcProject.ViewModels;
using Newtonsoft.Json;

namespace MvcProject.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly MvcDbContext _context;
        private readonly HttpClient _httpClient;

        public ReportsController(MvcDbContext context)
        {
            _context = context;
            _httpClient = new HttpClient();
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new ReportViewModel();

            // Local DB Stats (Users, Departments)
            viewModel.TotalUsers = await _context.Users.CountAsync();
            viewModel.TotalDepartments = await _context.Departments.CountAsync();

            // API Stats (Vehicles, Drivers, Depos)
            try
            {
                var vehicleResponse = await _httpClient.GetAsync("http://localhost:5234/api/Vehicles/GetVehicle");
                if (vehicleResponse.IsSuccessStatusCode)
                {
                    var vehicles = JsonConvert.DeserializeObject<List<Vehicle>>(await vehicleResponse.Content.ReadAsStringAsync()) ?? new List<Vehicle>();
                    viewModel.TotalVehicles = vehicles.Count;
                    viewModel.TotalVehicleCapacity = vehicles.Sum(v => v.Capacity);
                }

                var driverResponse = await _httpClient.GetAsync("http://localhost:5234/api/Drivers/GetDriver");
                if (driverResponse.IsSuccessStatusCode)
                {
                    var drivers = JsonConvert.DeserializeObject<List<Driver>>(await driverResponse.Content.ReadAsStringAsync()) ?? new List<Driver>();
                    viewModel.TotalDrivers = drivers.Count;
                }

                var depoResponse = await _httpClient.GetAsync("http://localhost:5234/api/Depos/GetDepo");
                if (depoResponse.IsSuccessStatusCode)
                {
                    var depos = JsonConvert.DeserializeObject<List<Depo>>(await depoResponse.Content.ReadAsStringAsync()) ?? new List<Depo>();
                    viewModel.TotalDepos = depos.Count;
                    viewModel.TotalDepoVolume = depos.Sum(d => d.Volume);
                }
            }
            catch (Exception)
            {
                // In case the API is offline, we'll just have zeros for API stats
            }

            return View(viewModel);
        }
    }
}
