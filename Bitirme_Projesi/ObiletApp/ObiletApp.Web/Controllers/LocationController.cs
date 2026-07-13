using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ObiletApp.Core.Entities;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ObiletApp.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class LocationController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _efApiUrl = "http://localhost:5108/api/Location";
        private readonly string _dapperApiUrl = "http://localhost:5028/api/LocationQuery";

        public LocationController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<IActionResult> Index()
        {
            var data = await _httpClient.GetFromJsonAsync<IEnumerable<Location>>(_dapperApiUrl);
            return View(data ?? new List<Location>());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Location entity)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpClient.PostAsJsonAsync(_efApiUrl, entity);
                if (response.IsSuccessStatusCode)
                    return RedirectToAction(nameof(Index));
            }
            return View(entity);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _httpClient.GetFromJsonAsync<Location>($"{_dapperApiUrl}/{id}");
            if (entity == null) return NotFound();
            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Location entity)
        {
            if (id != entity.Id) return BadRequest();
            if (ModelState.IsValid)
            {
                var response = await _httpClient.PutAsJsonAsync($"{_efApiUrl}/{id}", entity);
                if (response.IsSuccessStatusCode)
                    return RedirectToAction(nameof(Index));
            }
            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _httpClient.DeleteAsync($"{_efApiUrl}/{id}");
            return RedirectToAction(nameof(Index));
        }
    }
}