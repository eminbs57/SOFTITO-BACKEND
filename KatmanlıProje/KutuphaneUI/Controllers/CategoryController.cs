using Microsoft.AspNetCore.Mvc;
using Kutuphane.Data.Data;
using Microsoft.EntityFrameworkCore;
using Kutuphane.Model;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace KutuphaneUI.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;


        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string searchString)
        {
            var categories = _context.Categories.AsQueryable();
            if (!string.IsNullOrEmpty(searchString))
                categories = categories.Where(c => c.Name.Contains(searchString));
            return View(categories.ToList());
        }



        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Add(category);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null) return NotFound();
            return View(category);
        }
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Update(category);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        public IActionResult Delete(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null) return NotFound();
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}