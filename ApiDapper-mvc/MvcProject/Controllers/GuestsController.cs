using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using System;
using MvcProject.Models;
using MvcProject.Services;

namespace MvcProject.Controllers
{
    public class GuestsController : Controller
    {
        private readonly ApiService<Guest> _apiService;

        public GuestsController(ApiService<Guest> apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            var data = await _apiService.GetAllAsync();
            
            if (!string.IsNullOrEmpty(searchString))
            {
                data = data.Where(s => 
                    (s.FirstName != null && s.FirstName.Contains(searchString, StringComparison.OrdinalIgnoreCase)) || 
                    (s.LastName != null && s.LastName.Contains(searchString, StringComparison.OrdinalIgnoreCase)) ||
                    (s.IdentityNumber != null && s.IdentityNumber.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                );
            }
            
            ViewData["CurrentFilter"] = searchString;
            return View(data);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Guest entity)
        {
            await _apiService.CreateAsync(entity);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var data = await _apiService.GetByIdAsync(id);
            if (data == null) return NotFound();
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Guest entity)
        {
            await _apiService.UpdateAsync(id, entity);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _apiService.DeleteAsync(id);
            }
            catch (System.Exception)
            {
                TempData["ErrorMessage"] = "Bu müşteri kullanımda olduğu için silinemez (Örneğin aktif bir rezervasyonu olabilir).";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
