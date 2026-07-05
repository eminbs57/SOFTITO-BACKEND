using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using projectcodefirst.Models;

namespace projectcodefirst.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        
        private readonly AppDbContext _context;

       
        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        
        public IActionResult Index(string searchString)
        {
            var categories = _context.Categories.AsQueryable();
            if (!string.IsNullOrEmpty(searchString))
            {
                categories = categories.Where(c => c.Name.Contains(searchString));
            }
            return View(categories.ToList());
        }
        
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        public IActionResult Create(Category category)
        {
            
            _context.Categories.Add(category);

           
            _context.SaveChanges();

            
            return RedirectToAction("Index");
        }
       
        [HttpGet]
        public IActionResult Edit(int id)
        {
        
            var category = _context.Categories.Find(id);

            if (category == null)
                return NotFound(); 

            return View(category);
        }

        
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            _context.Categories.Update(category); 
            _context.SaveChanges();               
            return RedirectToAction("Index");     
        }


     
        public IActionResult Delete(int id)
        {
            var category = _context.Categories.Find(id);
            if (category != null)
            {
                _context.Categories.Remove(category); 
                _context.SaveChanges();               
            }
            return RedirectToAction("Index");
        }
    }
}