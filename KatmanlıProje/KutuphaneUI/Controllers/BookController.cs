using Kutuphane.Data.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Kutuphane.Model;

namespace KutuphaneUI.Controllers
{
    public class BookController : Controller
    {
        private readonly ApplicationDbContext _context;
        public BookController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string searchString)
        {
            var books = _context.Books.Include(b => b.Category).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {

                books = books.Where(b => b.Title.Contains(searchString) || b.Author.Contains(searchString));
            }

            return View(books.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Books.Add(book);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Name", book.CategoryId);
            return View(book);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var book = _context.Books.Find(id);
            if (book == null) return NotFound();

            ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Name", book.CategoryId);
            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Book book)
        {
            // 1. Veriyi veritabanından çek (Bunu yapınca EF Core takibe alır)
            var bookInDb = _context.Books.Find(book.Id);

            if (bookInDb == null) return NotFound();

            // 2. Yeni değerleri aktar
            bookInDb.Title = book.Title;
            bookInDb.Author = book.Author;
            bookInDb.CategoryId = book.CategoryId;
            bookInDb.Stock = book.Stock;

            // 3. Kaydet
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Hata varsa terminalde görelim
                Console.WriteLine("HATA: " + ex.Message);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var book = _context.Books.Find(id);
            if (book == null) return NotFound();
            _context.Books.Remove(book);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}