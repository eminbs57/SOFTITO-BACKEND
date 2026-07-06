using Microsoft.AspNetCore.Mvc;
using MvcProject.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MvcProject.Controllers
{
    public class ReportsController : Controller
    {
        private readonly string apiUrl = "http://localhost:5234/api/Reports";

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var viewModel = new ReportViewModel();
            
            using (HttpClient client = new HttpClient())
            {
                var responseTrips = await client.GetAsync(apiUrl + "/GetTripReport");
                if (responseTrips.IsSuccessStatusCode)
                {
                    var content = await responseTrips.Content.ReadAsStringAsync();
                    viewModel.Trips = JsonConvert.DeserializeObject<List<TripReportDto>>(content) ?? new List<TripReportDto>();
                }

                var responseDrivers = await client.GetAsync(apiUrl + "/GetDriverStats");
                if (responseDrivers.IsSuccessStatusCode)
                {
                    var content = await responseDrivers.Content.ReadAsStringAsync();
                    viewModel.DriverStats = JsonConvert.DeserializeObject<List<DriverStatDto>>(content) ?? new List<DriverStatDto>();
                }

                var responseRoutes = await client.GetAsync(apiUrl + "/GetRouteStats");
                if (responseRoutes.IsSuccessStatusCode)
                {
                    var content = await responseRoutes.Content.ReadAsStringAsync();
                    viewModel.RouteStats = JsonConvert.DeserializeObject<List<RouteStatDto>>(content) ?? new List<RouteStatDto>();
                }

                var responseVehicles = await client.GetAsync(apiUrl + "/GetVehicleStats");
                if (responseVehicles.IsSuccessStatusCode)
                {
                    var content = await responseVehicles.Content.ReadAsStringAsync();
                    viewModel.VehicleStats = JsonConvert.DeserializeObject<List<VehicleStatDto>>(content) ?? new List<VehicleStatDto>();
                }
            }

            return View(viewModel);
        }
    }
}
