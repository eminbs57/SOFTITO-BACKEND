using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using projectcodefirst.Models;

namespace projectcodefirst.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly AppDbContext _context;

        public CustomerController(AppDbContext context)
        {
            _context = context;
        }

        
        public IActionResult Index(string searchString)
        {
            var customers = _context.Customers.AsQueryable();
            if (!string.IsNullOrEmpty(searchString))
            {
                customers = customers.Where(c => c.FullName.Contains(searchString) || c.Email.Contains(searchString));
            }
            return View(customers.ToList());
        }
        
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Customer customer)
        {
            if (string.IsNullOrEmpty(customer.Password))
            {
                customer.Password = "123456"; // Default password since it's required by the model
            }
         
            _context.Customers.Add(customer);
            _context.SaveChanges();
        
            return RedirectToAction("Index");
        }

        

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var customer = _context.Customers.Find(id);
            if (customer == null) return NotFound();
            return View(customer);
        }

        [HttpPost]
        public IActionResult Edit(Customer customer)
        {
            var existing = _context.Customers.Find(customer.Id);
            if (existing != null)
            {
                existing.FullName = customer.FullName;
                existing.Email = customer.Email;
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        

        public IActionResult Delete(int id)
        {
            var customer = _context.Customers.Find(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}