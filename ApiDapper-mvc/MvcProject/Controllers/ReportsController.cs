using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcProject.Models;
using Newtonsoft.Json;

namespace MvcProject.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly HttpClient _httpClient;

        public ReportsController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new System.Uri("http://localhost:5055/");
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new ReportsViewModel();

            try
            {
                // API'den tüm raporları paralel olarak çekelim (Performans için)
                var task1 = _httpClient.GetAsync("api/reports/roomtyperevenue");
                var task2 = _httpClient.GetAsync("api/reports/gueststays");
                var task3 = _httpClient.GetAsync("api/reports/roomavailability");
                var task4 = _httpClient.GetAsync("api/reports/upcomingreservations");
                var task5 = _httpClient.GetAsync("api/reports/monthlyrevenue");

                await Task.WhenAll(task1, task2, task3, task4, task5);

                if (task1.Result.IsSuccessStatusCode)
                    viewModel.RoomTypeRevenues = JsonConvert.DeserializeObject<List<RoomTypeRevenueDto>>(await task1.Result.Content.ReadAsStringAsync());

                if (task2.Result.IsSuccessStatusCode)
                    viewModel.GuestStays = JsonConvert.DeserializeObject<List<GuestStayDto>>(await task2.Result.Content.ReadAsStringAsync());

                if (task3.Result.IsSuccessStatusCode)
                    viewModel.RoomAvailabilities = JsonConvert.DeserializeObject<List<RoomAvailabilityDto>>(await task3.Result.Content.ReadAsStringAsync());

                if (task4.Result.IsSuccessStatusCode)
                    viewModel.UpcomingReservations = JsonConvert.DeserializeObject<List<UpcomingReservationDto>>(await task4.Result.Content.ReadAsStringAsync());

                if (task5.Result.IsSuccessStatusCode)
                    viewModel.MonthlyRevenues = JsonConvert.DeserializeObject<List<MonthlyRevenueDto>>(await task5.Result.Content.ReadAsStringAsync());
            }
            catch (System.Exception ex)
            {
                TempData["ErrorMessage"] = "Raporlar yüklenirken bir hata oluştu: " + ex.Message;
            }

            return View(viewModel);
        }
    }
}
