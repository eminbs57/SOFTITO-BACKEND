using Microsoft.AspNetCore.Mvc;
using EventManagement.Data;
using EventManagement.Models;
using Microsoft.AspNetCore.Authorization;

namespace EventManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index() => View();

        public JsonResult Edit(int id)
        {
            var data = _context.Categories.Where(m => m.Id == id).SingleOrDefault();
            return new JsonResult(data);
        }

        [HttpPost]
        public JsonResult Update(Category category)
        {
            _context.Update(category);
            _context.SaveChanges();
            return new JsonResult("Kayıt güncellendi");
        }


        public JsonResult SearchCategory(string searchString)
        {
            var data = _context.Categories.AsQueryable();
            if (!string.IsNullOrEmpty(searchString))
            {
                data = data.Where(c => c.Name.Contains(searchString));
            }
            return new JsonResult(data.ToList());
        }

        public JsonResult CategoryList() => new JsonResult(_context.Categories.ToList());

        [HttpPost]
        public JsonResult AddCategory(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
            return new JsonResult("Başarıyla kaydedildi");
        }

        public JsonResult Delete(int id)
        {
            var data = _context.Categories.Find(id);
            _context.Categories.Remove(data);
            _context.SaveChanges();
            return new JsonResult("Kayıt silindi");
        }
    }
}