using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ObiletApp.Web.Models;

namespace ObiletApp.Web.Controllers;

    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var locationsResponse = await _httpClient.GetStringAsync("http://localhost:5028/api/LocationQuery");
                var locations = System.Text.Json.JsonSerializer.Deserialize<System.Collections.Generic.List<dynamic>>(locationsResponse);
                ViewBag.Locations = locations;
            }
            catch
            {
                ViewBag.Locations = new System.Collections.Generic.List<dynamic>();
            }
            return View();
        }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
