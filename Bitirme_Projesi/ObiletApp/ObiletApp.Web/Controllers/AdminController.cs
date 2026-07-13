using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ObiletApp.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly HttpClient _httpClient;

        public AdminController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _httpClient.GetStringAsync("http://localhost:5028/api/DashboardQuery");
                var stats = System.Text.Json.JsonDocument.Parse(response).RootElement;

                ViewBag.TotalTickets = stats.GetProperty("totalTicketsSold").GetInt32();
                ViewBag.ActiveRoutes = stats.GetProperty("activeTrips").GetInt32();
                ViewBag.DailyRevenue = $"{stats.GetProperty("dailyRevenue").GetDecimal():N2} ₺";
                ViewBag.TotalCompanies = stats.GetProperty("totalCompanies").GetInt32();

                if (stats.TryGetProperty("recentTickets", out var recentTicketsProp))
                {
                    ViewBag.RecentTickets = System.Text.Json.JsonSerializer.Deserialize<System.Collections.Generic.List<dynamic>>(recentTicketsProp.GetRawText());
                }
            }
            catch
            {
                ViewBag.TotalTickets = 0;
                ViewBag.ActiveRoutes = 0;
                ViewBag.DailyRevenue = "0,00 ₺";
                ViewBag.TotalCompanies = 0;
            }

            return View();
        }

        public IActionResult DownloadPdf()
        {

            return Redirect("https://localhost:7198/api/ticket/report/pdf");
        }

        public IActionResult DownloadExcel()
        {
            return Redirect("https://localhost:7198/api/ticket/report/excel");
        }
    }
}
