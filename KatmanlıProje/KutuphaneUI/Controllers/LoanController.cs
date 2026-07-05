using Kutuphane.Data.Data;
using Kutuphane.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClosedXML.Excel;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace KutuphaneUI.Controllers
{
    public class LoanController : Controller
    {
        private readonly ApplicationDbContext _context;
        public LoanController(ApplicationDbContext context)
        {
            _context = context;
        }
        // LoanController.cs
        public IActionResult Index(string searchString)
        {
            var loans = _context.Loans.Include(l => l.Book).Include(l => l.User).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                loans = loans.Where(l => l.Book.Title.Contains(searchString) || l.User.Name.Contains(searchString));
            }
            return View(loans.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.BookId = new SelectList(_context.Books, "Id", "Title");
            ViewBag.UserId = new SelectList(_context.Users, "Id", "Name");
            return View();
        }
        [HttpPost]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Loan loan)
        {
            // 1. Kitabı bulalım
            var book = _context.Books.Find(loan.BookId);

            // 2. Stok Kontrolü (Hata mesajını burada veriyoruz)
            if (book != null && book.Stock <= 0)
            {
                ModelState.AddModelError("BookId", "Üzgünüz, bu kitap için yeterli stok bulunmuyor.");
            }

            // 3. ModelState geçerliyse kayıt yap
            if (ModelState.IsValid)
            {
                // Stoktan 1 düş
                book.Stock -= 1;

                _context.Loans.Add(loan);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            // Hata varsa veya stok yoksa, dropdownları tekrar doldurup sayfayı döndür
            ViewBag.BookId = new SelectList(_context.Books, "Id", "Title");
            ViewBag.UserId = new SelectList(_context.Users, "Id", "Name");

            return View(loan);
        }
        public IActionResult Delete(int id)
        {
            // Kaydı ve ilişkili kitabı çek
            var loan = _context.Loans.Include(l => l.Book).FirstOrDefault(l => l.Id == id);

            if (loan != null)
            {
                // Stok geri artırıldı
                loan.Book.Stock += 1;

                _context.Loans.Remove(loan);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var loan = _context.Loans.Find(id);
            ViewBag.BookId = new SelectList(_context.Books, "Id", "Title", loan.BookId);
            ViewBag.UserId = new SelectList(_context.Users, "Id", "Name", loan.UserId);
            return View(loan);
        }

        public IActionResult ExportToExcel()
        {
            var loans = _context.Loans.Include(l => l.Book).Include(l => l.User).ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Ödünç İşlemleri");
                var currentRow = 1;

                worksheet.Cell(currentRow, 1).Value = "Ödünç Verilen Kitaplar";
                worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
                currentRow++;

                worksheet.Cell(currentRow, 1).Value = "Kitap";
                worksheet.Cell(currentRow, 2).Value = "Kullanıcı";
                worksheet.Cell(currentRow, 3).Value = "Veriliş Tarihi";
                worksheet.Cell(currentRow, 4).Value = "Kalan Stok";

                foreach (var item in loans)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = item.Book?.Title;
                    worksheet.Cell(currentRow, 2).Value = item.User?.Name;
                    worksheet.Cell(currentRow, 3).Value = item.LoanDate.ToShortDateString();
                    worksheet.Cell(currentRow, 4).Value = item.Book?.Stock;
                }

                using (var stream = new System.IO.MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "OduncIslemleri.xlsx");
                }
            }
        }

        public IActionResult ExportToPdf()
        {
            QuestPDF.Settings.License = LicenseType.Community;
            var loans = _context.Loans.Include(l => l.Book).Include(l => l.User).ToList();

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header().Text("Kütüphane Yönetim Paneli - Ödünç İşlemleri")
                        .SemiBold().FontSize(20).FontColor(Colors.Blue.Darken2);

                    page.Content().PaddingVertical(1, Unit.Centimetre).Column(x =>
                    {
                        x.Item().PaddingBottom(10).Text("Ödünç Verilen Kitaplar Listesi").Bold().FontSize(14);
                        x.Item().Table(t =>
                        {
                            t.ColumnsDefinition(c =>
                            {
                                c.RelativeColumn(2);
                                c.RelativeColumn(2);
                                c.RelativeColumn(1);
                                c.RelativeColumn(1);
                            });
                            t.Header(h =>
                            {
                                h.Cell().Element(Block).Text("Kitap");
                                h.Cell().Element(Block).Text("Kullanıcı");
                                h.Cell().Element(Block).Text("Veriliş Tarihi");
                                h.Cell().Element(Block).Text("Kalan Stok");
                            });
                            foreach (var item in loans)
                            {
                                t.Cell().Element(Block).Text(item.Book?.Title);
                                t.Cell().Element(Block).Text(item.User?.Name);
                                t.Cell().Element(Block).Text(item.LoanDate.ToShortDateString());
                                t.Cell().Element(Block).Text(item.Book?.Stock.ToString());
                            }
                        });

                        static IContainer Block(IContainer container)
                        {
                            return container
                                .BorderBottom(1)
                                .BorderColor(Colors.Grey.Lighten2)
                                .PaddingVertical(5);
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

            byte[] pdfBytes = document.GeneratePdf();
            return File(pdfBytes, "application/pdf", "OduncIslemleri.pdf");
        }
    }
}