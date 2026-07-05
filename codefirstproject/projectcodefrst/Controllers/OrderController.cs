using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using projectcodefirst.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
namespace projectcodefirst.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;

        public OrderController(AppDbContext context)
        {
            _context = context;
        }

       
        public IActionResult Index(string searchString)
        {
           
            var orders = _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Product)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                orders = orders.Where(o => o.Customer.FullName.Contains(searchString) || o.Product.Name.Contains(searchString));
            }

            return View(orders.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {

            ViewBag.Customers = _context.Customers.ToList();
            ViewBag.Products = _context.Products.ToList();

            return View();
        }

        [HttpPost]
        public IActionResult Create(Order order)
        {
            
            _context.Orders.Add(order);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

       

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var order = _context.Orders.Find(id);
            if (order == null) return NotFound();

           
            ViewBag.Customers = _context.Customers.ToList();
            ViewBag.Products = _context.Products.ToList();

            return View(order);
        }

        [HttpPost]
        public IActionResult Edit(Order order)
        {
            _context.Orders.Update(order);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

       

        public IActionResult Delete(int id)
        {
            var order = _context.Orders.Find(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult ExportToPdf()
        {
            var orders = _context.Orders.Include(o => o.Customer).Include(o => o.Product).ToList();
            var pdfDocument = QuestPDF.Fluent.Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(QuestPDF.Helpers.PageSizes.A4);
                    page.Margin(2, QuestPDF.Infrastructure.Unit.Centimetre);
                    page.PageColor(QuestPDF.Helpers.Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Arial"));

                    page.Header()
                        .Text("Sipariş Listesi Raporu")
                        .SemiBold().FontSize(20).FontColor(QuestPDF.Helpers.Colors.Blue.Medium);

                    page.Content()
                        .PaddingTop(1, QuestPDF.Infrastructure.Unit.Centimetre)
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(50);
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.ConstantColumn(100);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Background(QuestPDF.Helpers.Colors.Grey.Lighten2).Padding(5).Text("ID").Bold();
                                header.Cell().Background(QuestPDF.Helpers.Colors.Grey.Lighten2).Padding(5).Text("Müşteri").Bold();
                                header.Cell().Background(QuestPDF.Helpers.Colors.Grey.Lighten2).Padding(5).Text("Ürün").Bold();
                                header.Cell().Background(QuestPDF.Helpers.Colors.Grey.Lighten2).Padding(5).Text("Miktar").Bold();
                            });

                            foreach (var item in orders)
                            {
                                table.Cell().BorderBottom(1).BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten3).Padding(5).Text(item.Id.ToString());
                                table.Cell().BorderBottom(1).BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten3).Padding(5).Text(item.Customer != null ? item.Customer.FullName : "-");
                                table.Cell().BorderBottom(1).BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten3).Padding(5).Text(item.Product != null ? item.Product.Name : "-");
                                table.Cell().BorderBottom(1).BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten3).Padding(5).Text(item.Quantity.ToString());
                            }
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Sayfa ");
                            x.CurrentPageNumber();
                        });
                });
            });

            var pdfBytes = pdfDocument.GeneratePdf();
            return File(pdfBytes, "application/pdf", $"Siparis_Listesi_{DateTime.Now:yyyyMMdd}.pdf");
        }

        public IActionResult ExportToExcel()
        {
            OfficeOpenXml.ExcelPackage.License.SetNonCommercialPersonal("Backend softito");
            var orders = _context.Orders.Include(o => o.Customer).Include(o => o.Product).ToList();

            using (var package = new OfficeOpenXml.ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sipariş Listesi");

                worksheet.Cells[1, 1].Value = "Sipariş ID";
                worksheet.Cells[1, 2].Value = "Müşteri";
                worksheet.Cells[1, 3].Value = "Ürün";
                worksheet.Cells[1, 4].Value = "Miktar";

                using (var range = worksheet.Cells[1, 1, 1, 4]) 
                {
                    range.Style.Font.Bold = true; 
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(41, 128, 185)); 
                    range.Style.Font.Color.SetColor(System.Drawing.Color.White); 
                    range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; 
                }

                int rowNumber = 2; 
                foreach (var item in orders)
                {
                    worksheet.Cells[rowNumber, 1].Value = item.Id;
                    worksheet.Cells[rowNumber, 2].Value = item.Customer != null ? item.Customer.FullName : "-";
                    worksheet.Cells[rowNumber, 3].Value = item.Product != null ? item.Product.Name : "-";
                    worksheet.Cells[rowNumber, 4].Value = item.Quantity;

                    rowNumber++;
                }

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                var fileBytes = package.GetAsByteArray();
                string fileName = $"Siparis_Listesi_{DateTime.Now:yyyyMMdd}.xlsx";

                return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }
    }
}