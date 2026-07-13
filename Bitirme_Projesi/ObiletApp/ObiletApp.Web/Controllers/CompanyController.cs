using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ObiletApp.Core.Entities;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ObiletApp.Web.Controllers
{

    public class CompanyController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CompanyController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var dapperClient = _httpClientFactory.CreateClient("DapperApi");
            var response = await dapperClient.GetAsync("CompanyQuery");

            if (response.IsSuccessStatusCode)
            {
                var companies = await response.Content.ReadFromJsonAsync<IEnumerable<Company>>();
                return View(companies);
            }

            return View(new List<Company>());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Company company)
        {
            if (ModelState.IsValid)
            {
                var efClient = _httpClientFactory.CreateClient("EfApi");
                company.CreatedDate = System.DateTime.UtcNow;
                company.IsActive = true;

                var response = await efClient.PostAsJsonAsync("Company", company);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Firma başarıyla eklendi!";
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Firma eklenirken API tarafında bir hata oluştu.");
            }
            return View(company);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var dapperClient = _httpClientFactory.CreateClient("DapperApi");
            var response = await dapperClient.GetAsync($"CompanyQuery/{id}");

            if (response.IsSuccessStatusCode)
            {
                var company = await response.Content.ReadFromJsonAsync<Company>();
                if (company == null) return NotFound();
                return View(company);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Company company)
        {
            if (id != company.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var efClient = _httpClientFactory.CreateClient("EfApi");
                company.UpdatedDate = System.DateTime.UtcNow;

                var response = await efClient.PutAsJsonAsync($"Company/{id}", company);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Firma başarıyla güncellendi!";
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Firma güncellenirken API tarafında bir hata oluştu.");
            }
            return View(company);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var efClient = _httpClientFactory.CreateClient("EfApi");
            var response = await efClient.DeleteAsync($"Company/{id}");

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Firma başarıyla silindi!";
            }
            else
            {
                TempData["ErrorMessage"] = "Firma silinemedi.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
