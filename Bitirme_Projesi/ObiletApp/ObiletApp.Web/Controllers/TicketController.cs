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
    public class TicketController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _efApiUrl = "http://localhost:5108/api/Ticket";
        private readonly string _dapperApiUrl = "http://localhost:5028/api/TicketQuery";

        public TicketController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<IActionResult> Index()
        {
            var data = await _httpClient.GetFromJsonAsync<IEnumerable<Ticket>>(_dapperApiUrl);
            return View(data ?? new List<Ticket>());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Ticket entity)
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
            var entity = await _httpClient.GetFromJsonAsync<Ticket>($"{_dapperApiUrl}/{id}");
            if (entity == null) return NotFound();
            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Ticket entity)
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