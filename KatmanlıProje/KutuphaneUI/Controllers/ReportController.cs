using Kutuphane.Data.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Kutuphane.Model;


namespace KutuphaneUI.Controllers
{
    public class ReportController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ReportController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {

            ViewBag.OutOfStock = _context.Books.Where(b => b.Stock <= 0).ToList();


            ViewBag.CategoryStats = _context.Books
                .GroupBy(b => b.Category.Name)
                .Select(g => new { CategoryName = g.Key, TotalStock = g.Sum(b => b.Stock) })
                .ToList();


            ViewBag.LoanUserReport = _context.Loans
                .Join(_context.Books, l => l.BookId, b => b.Id, (l, b) => new { l, b })
                .Join(_context.Users, combined => combined.l.UserId, u => u.Id, (combined, u) => new
                {
                    Kitap = combined.b.Title,
                    Kullanici = u.Name,
                    Tarih = combined.l.LoanDate
                }).ToList();


            ViewBag.BookCategoryReport = _context.Books
                .Join(_context.Categories, b => b.CategoryId, c => c.Id, (b, c) => new
                {
                    Kitap = b.Title,
                    Kategori = c.Name,
                    Stok = b.Stock
                }).ToList();


            ViewBag.CategoryLoanCountReport = _context.Loans
                .Join(_context.Books, l => l.BookId, b => b.Id, (l, b) => new { l, b })
                .Join(_context.Categories, combined => combined.b.CategoryId, c => c.Id, (combined, c) => new
                {
                    Kategori = c.Name,
                    Kitap = combined.b.Title
                }).ToList();

            ViewBag.PopularBooks = _context.Loans
                .GroupBy(l => l.Book.Title)
                .OrderByDescending(g => g.Count())
                .Take(5)
                .Select(g => new { Title = g.Key, Count = g.Count() }).ToList();

            ViewBag.ActiveLoans = _context.Loans
                .Select(l => new { l.Book.Title, l.User.Name, l.LoanDate }).ToList();

            ViewBag.TopUsers = _context.Loans
                .GroupBy(l => l.User.Name)
                .OrderByDescending(g => g.Count())
                .Take(5)
                .Select(g => new { Name = g.Key, Count = g.Count() }).ToList();

            return View();
        }
    }
}