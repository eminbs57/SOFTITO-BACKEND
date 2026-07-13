using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ObiletApp.Web.Models;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ObiletApp.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReportController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _dapperApiUrl = "http://localhost:5028/api/ReportQuery"; 

        public ReportController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new ReportDashboardViewModel();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            try
            {

                var response1 = await _httpClient.GetAsync($"{_dapperApiUrl}/CompanyRevenue");
                if (response1.IsSuccessStatusCode)
                    viewModel.CompanyRevenues = JsonSerializer.Deserialize<List<ReportCompanyRevenueDto>>(await response1.Content.ReadAsStringAsync(), options) ?? new List<ReportCompanyRevenueDto>();

                var response2 = await _httpClient.GetAsync($"{_dapperApiUrl}/RouteTraffic");
                if (response2.IsSuccessStatusCode)
                    viewModel.RouteTraffic = JsonSerializer.Deserialize<List<ReportRouteTrafficDto>>(await response2.Content.ReadAsStringAsync(), options) ?? new List<ReportRouteTrafficDto>();

                var response3 = await _httpClient.GetAsync($"{_dapperApiUrl}/UserLoyalty");
                if (response3.IsSuccessStatusCode)
                    viewModel.UserLoyalty = JsonSerializer.Deserialize<List<ReportUserLoyaltyDto>>(await response3.Content.ReadAsStringAsync(), options) ?? new List<ReportUserLoyaltyDto>();

                var response4 = await _httpClient.GetAsync($"{_dapperApiUrl}/VehicleOccupancy");
                if (response4.IsSuccessStatusCode)
                    viewModel.VehicleOccupancy = JsonSerializer.Deserialize<List<ReportVehicleOccupancyDto>>(await response4.Content.ReadAsStringAsync(), options) ?? new List<ReportVehicleOccupancyDto>();

                var response5 = await _httpClient.GetAsync($"{_dapperApiUrl}/MonthlySales");
                if (response5.IsSuccessStatusCode)
                    viewModel.MonthlySales = JsonSerializer.Deserialize<List<ReportMonthlySalesDto>>(await response5.Content.ReadAsStringAsync(), options) ?? new List<ReportMonthlySalesDto>();

                var response6 = await _httpClient.GetAsync($"{_dapperApiUrl}/TicketStatus");
                if (response6.IsSuccessStatusCode)
                    viewModel.TicketStatus = JsonSerializer.Deserialize<List<ReportTicketStatusDto>>(await response6.Content.ReadAsStringAsync(), options) ?? new List<ReportTicketStatusDto>();

                var response7 = await _httpClient.GetAsync($"{_dapperApiUrl}/UpcomingTrips");
                if (response7.IsSuccessStatusCode)
                    viewModel.UpcomingTrips = JsonSerializer.Deserialize<List<ReportUpcomingTripDto>>(await response7.Content.ReadAsStringAsync(), options) ?? new List<ReportUpcomingTripDto>();

                var response8 = await _httpClient.GetAsync($"{_dapperApiUrl}/LocationTraffic");
                if (response8.IsSuccessStatusCode)
                    viewModel.LocationTraffic = JsonSerializer.Deserialize<List<ReportLocationTrafficDto>>(await response8.Content.ReadAsStringAsync(), options) ?? new List<ReportLocationTrafficDto>();
            }
            catch
            {

                ModelState.AddModelError("", "Rapor verileri çekilirken bir hata oluştu.");
            }

            return View(viewModel);
        }
    }
}
