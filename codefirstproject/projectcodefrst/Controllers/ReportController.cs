using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using projectcodefirst.Models;

namespace projectcodefirst.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private readonly AppDbContext _context;

        public ReportController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult TopCustomers()
        {
            var report = _context.Orders
                .Include(o => o.Customer)
                .GroupBy(o => new { o.Customer.Id, o.Customer.FullName })
                .Select(g => new 
                {
                    CustomerName = g.Key.FullName,
                    TotalOrders = g.Count(),
                    TotalItemsBought = g.Sum(o => o.Quantity)
                })
                .OrderByDescending(x => x.TotalOrders)
                .Take(10)
                .ToList();

            ViewBag.Report = report;
            return View();
        }

        public IActionResult CategorySales()
        {
            var report = _context.Orders
                .Include(o => o.Product)
                .ThenInclude(p => p.Category)
                .GroupBy(o => o.Product.Category.Name)
                .Select(g => new 
                {
                    CategoryName = g.Key,
                    TotalSoldQuantity = g.Sum(o => o.Quantity),
                    TotalRevenue = g.Sum(o => o.Quantity * o.Product.Price)
                })
                .OrderByDescending(x => x.TotalRevenue)
                .ToList();

            ViewBag.Report = report;
            return View();
        }

        public IActionResult ProductStock()
        {
            var report = _context.Products
                .Include(p => p.Category)
                .OrderBy(p => p.Stock)
                .ToList();

            return View(report);
        }

        public IActionResult TopSellingProducts()
        {
            var report = _context.Orders
                .Include(o => o.Product)
                .GroupBy(o => new { o.Product.Id, o.Product.Name })
                .Select(g => new 
                {
                    ProductName = g.Key.Name,
                    TotalQuantity = g.Sum(o => o.Quantity),
                    TotalRevenue = g.Sum(o => o.Quantity * o.Product.Price)
                })
                .OrderByDescending(x => x.TotalQuantity)
                .Take(10)
                .ToList();

            ViewBag.Report = report;
            return View();
        }

        public IActionResult RecentOrders()
        {
            var report = _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Product)
                .OrderByDescending(o => o.OrderDate)
                .Take(20)
                .ToList();

            return View(report);
        }
    }
}
