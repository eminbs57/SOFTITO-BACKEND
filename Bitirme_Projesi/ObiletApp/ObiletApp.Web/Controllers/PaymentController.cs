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
    public class PaymentController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _efApiUrl = "http://localhost:5108/api/Payment";
        private readonly string _dapperApiUrl = "http://localhost:5028/api/PaymentQuery";

        public PaymentController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<IActionResult> Index()
        {
            var data = await _httpClient.GetFromJsonAsync<IEnumerable<Payment>>(_dapperApiUrl);
            return View(data ?? new List<Payment>());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Payment entity)
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
            var entity = await _httpClient.GetFromJsonAsync<Payment>($"{_dapperApiUrl}/{id}");
            if (entity == null) return NotFound();
            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Payment entity)
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