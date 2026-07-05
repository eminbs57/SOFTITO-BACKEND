using Microsoft.AspNetCore.Mvc;
using EventManagement.Data;
using EventManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IO;

namespace EventManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EventController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<EventController> _logger;

        public EventController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, ILogger<EventController> logger)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }


        public JsonResult EventList()
        {
            var data = _context.Events.ToList();
            return new JsonResult(data);
        }


        [HttpPost]
        public async Task<IActionResult> AddEvent(Event @event, IFormFile? imageFile)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Etkinlik eklenirken ModelState hatası yaşandı.");
                return BadRequest("Lütfen tüm zorunlu alanları (Başlık, Tarih, Kontenjan) doldurduğunuzdan emin olun.");
            }
            
            string? uniqueFileName = null;
            if (imageFile != null && imageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/events");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);
                
                uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }
            }

            var newEvent = new Event()
            {
                Title = @event.Title,
                Description = @event.Description,
                Date = @event.Date,
                CategoryName = @event.CategoryName,
                Kota = @event.Kota,
                ImageUrl = uniqueFileName != null ? "/images/events/" + uniqueFileName : null
            };
            
            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();
            
            _logger.LogInformation("Yeni bir etkinlik başarıyla eklendi: {Title}", newEvent.Title);
            return Ok("Başarıyla kaydedildi");
        }


        public JsonResult Edit(int id)
        {
            var data = _context.Events.Where(m => m.Id == id).SingleOrDefault();
            return new JsonResult(data);
        }


        [HttpPost]
        public async Task<IActionResult> Update(Event @event, IFormFile? imageFile)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Etkinlik güncellenirken ModelState hatası yaşandı. ID: {EventId}", @event.Id);
                return BadRequest("Lütfen tüm zorunlu alanları (Başlık, Tarih, Kontenjan, Açıklama) doldurduğunuzdan emin olun.");
            }
            
            var existingEvent = await _context.Events.FindAsync(@event.Id);
            if (existingEvent == null) return NotFound();

            if (imageFile != null && imageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/events");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);
                
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }
                existingEvent.ImageUrl = "/images/events/" + uniqueFileName;
            }

            existingEvent.Title = @event.Title;
            existingEvent.Description = @event.Description;
            existingEvent.Date = @event.Date;
            existingEvent.CategoryName = @event.CategoryName;
            existingEvent.Kota = @event.Kota;

            _context.Update(existingEvent);
            await _context.SaveChangesAsync();
            
            _logger.LogInformation("Etkinlik başarıyla güncellendi: {Title}", existingEvent.Title);
            return Ok("Kayıt güncellendi");
        }


        public JsonResult Delete(int id)
        {
            var data = _context.Events.Where(m => m.Id == id).SingleOrDefault();
            if (data != null)
            {
                _context.Events.Remove(data);
                _context.SaveChanges();
            }
            return new JsonResult("Kayıt silindi");
        }
        public JsonResult SearchEvent(string searchString)
        {
            var data = _context.Events.AsQueryable();
            if (!string.IsNullOrEmpty(searchString))
            {
                data = data.Where(e => e.Title.Contains(searchString) || e.CategoryName.Contains(searchString));
            }
            return new JsonResult(data.ToList());
        }
    }
}