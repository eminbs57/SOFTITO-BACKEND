using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;
using MvcProject.Models;
using MvcProject.Services;

namespace MvcProject.Controllers
{
    public class RoomsController : Controller
    {
        private readonly ApiService<Room> _apiService;
        private readonly ApiService<RoomType> _roomTypeService;

        public RoomsController(ApiService<Room> apiService, ApiService<RoomType> roomTypeService)
        {
            _apiService = apiService;
            _roomTypeService = roomTypeService;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            var data = await _apiService.GetAllAsync();
            
            if (!string.IsNullOrEmpty(searchString))
            {
                data = data.Where(s => 
                    (s.RoomNumber != null && s.RoomNumber.Contains(searchString, StringComparison.OrdinalIgnoreCase)) ||
                    (s.RoomTypeName != null && s.RoomTypeName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                );
            }

            ViewData["CurrentFilter"] = searchString;
            return View(data);
        }

        public async Task<IActionResult> Create()
        {
            var roomTypes = await _roomTypeService.GetAllAsync();
            ViewBag.RoomTypes = new SelectList(roomTypes, "Id", "Name");

            var allRooms = await _apiService.GetAllAsync();
            var existingNumbers = allRooms.Select(r => r.RoomNumber).ToList();
            var potentialNumbers = new List<string>();
            for(int i = 101; i <= 120; i++) {
                if(!existingNumbers.Contains(i.ToString())) {
                    potentialNumbers.Add(i.ToString());
                }
            }
            ViewBag.AvailableRoomNumbers = new SelectList(potentialNumbers);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Room entity)
        {
            await _apiService.CreateAsync(entity);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var data = await _apiService.GetByIdAsync(id);
            if (data == null) return NotFound();
            var roomTypes = await _roomTypeService.GetAllAsync();
            ViewBag.RoomTypes = new SelectList(roomTypes, "Id", "Name", data.RoomTypeId);
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Room entity)
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
                TempData["ErrorMessage"] = "Bu oda kullanımda olduğu için silinemez (Örneğin aktif bir rezervasyonu olabilir).";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
