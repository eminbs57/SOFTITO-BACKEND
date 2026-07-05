using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using dbfirstProjem.Models;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Collections.Generic;

namespace dbfirstProjem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IMemoryCache _cache;
        private readonly ILogger<AdminController> _logger;

        public AdminController(ApplicationDbContext context, IWebHostEnvironment env, IMemoryCache cache, ILogger<AdminController> logger)
        {
            _context = context;
            _env = env;
            _cache = cache;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // --- FİLMLER ---
        public IActionResult Index(string searchString)
        {
            string cacheKey = "filmList";

            if (!_cache.TryGetValue(cacheKey, out List<Filmler> filmler))
            {
                _logger.LogWarning(">>> VERİLER VERİTABANINDAN ÇEKİLDİ VE CACHE'E EKLENDİ! <<<");
                filmler = _context.Filmlers.ToList();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));

                _cache.Set(cacheKey, filmler, cacheOptions);
            }
            else
            {
                _logger.LogInformation(">>> VERİLER CACHE'TEN (BELLEKTEN) GELDİ! (Hızlı Yükleme) <<<");
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                filmler = filmler.Where(f => f.Ad != null && f.Ad.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            _logger.LogInformation("Toplam {Count} adet film listelendi.", filmler?.Count ?? 0);
            return View(filmler);
        }

        public IActionResult ExportToPdf_Filmler()
        {
            var filmler = _context.Filmlers.ToList();
            var pdfDocument = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Arial"));

                    page.Header()
                        .Text("Film Listesi Raporu")
                        .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                    page.Content()
                        .PaddingTop(1, Unit.Centimetre)
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(50);
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.ConstantColumn(80);
                                columns.ConstantColumn(80);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("ID").Bold();
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Film Adı").Bold();
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Türü").Bold();
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Süresi").Bold();
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Fiyatı").Bold();
                            });

                            foreach (var item in filmler)
                            {
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.FilmId.ToString());
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.Ad ?? "");
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.Tur ?? "");
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.SureDk.ToString() + " Dk");
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.Fiyat?.ToString("C") ?? "-");
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
            return File(pdfBytes, "application/pdf", $"Film_Listesi_{DateTime.Now:yyyyMMdd}.pdf");
        }

        public IActionResult ExportToExcel_Filmler()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var filmler = _context.Filmlers.ToList();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Film Listesi");

                worksheet.Cells[1, 1].Value = "Film ID";
                worksheet.Cells[1, 2].Value = "Film Adı";
                worksheet.Cells[1, 3].Value = "Türü";
                worksheet.Cells[1, 4].Value = "Süresi (Dk)";
                worksheet.Cells[1, 5].Value = "Fiyatı";

                using (var range = worksheet.Cells[1, 1, 1, 5])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(41, 128, 185));
                    range.Style.Font.Color.SetColor(System.Drawing.Color.White);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                int rowNumber = 2;
                foreach (var item in filmler)
                {
                    worksheet.Cells[rowNumber, 1].Value = item.FilmId;
                    worksheet.Cells[rowNumber, 2].Value = item.Ad;
                    worksheet.Cells[rowNumber, 3].Value = item.Tur;
                    worksheet.Cells[rowNumber, 4].Value = item.SureDk;
                    worksheet.Cells[rowNumber, 5].Value = item.Fiyat;
                    rowNumber++;
                }

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                var fileBytes = package.GetAsByteArray();
                string fileName = $"Film_Listesi_{DateTime.Now:yyyyMMdd}.xlsx";

                return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }

        [HttpGet]
        public IActionResult FilmEkle()
        {
            return View(new Filmler());
        }

        [HttpPost]
        public IActionResult FilmEkle(Filmler yeniFilm, IFormFile? file)
        {
            ModelState.Remove("ResimUrl");
            ModelState.Remove("Seanslars");
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    string imagePath = Path.Combine(_env.WebRootPath, @"img/movies");
                    if (!Directory.Exists(imagePath)) Directory.CreateDirectory(imagePath);
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    using (var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    yeniFilm.ResimUrl = @"/img/movies/" + fileName;
                }
                else { yeniFilm.ResimUrl = ""; }

                _context.Filmlers.Add(yeniFilm);
                _context.SaveChanges();
                _cache.Remove("filmList");
                TempData["Success"] = "Film başarıyla eklendi.";
                return RedirectToAction("Index");
            }
            return View(yeniFilm);
        }

        [HttpGet]
        public IActionResult FilmDuzenle(int id)
        {
            var film = _context.Filmlers.Find(id);
            if (film == null) return NotFound();
            return View(film);
        }

        [HttpPost]
        public IActionResult FilmDuzenle(Filmler guncelFilm, IFormFile? file)
        {
            ModelState.Remove("ResimUrl");
            ModelState.Remove("Seanslars");
            if (ModelState.IsValid)
            {
                var film = _context.Filmlers.Find(guncelFilm.FilmId);
                if (film == null) return NotFound();

                film.Ad = guncelFilm.Ad;
                film.Tur = guncelFilm.Tur;
                film.SureDk = guncelFilm.SureDk;
                film.Fiyat = guncelFilm.Fiyat;

                if (file != null)
                {
                    string imagePath = Path.Combine(_env.WebRootPath, @"img/movies");
                    if (!Directory.Exists(imagePath)) Directory.CreateDirectory(imagePath);
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    using (var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    film.ResimUrl = @"/img/movies/" + fileName;
                }

                _context.SaveChanges();
                _cache.Remove("filmList");
                TempData["Success"] = "Film başarıyla güncellendi.";
                return RedirectToAction("Index");
            }
            return View(guncelFilm);
        }

        public IActionResult FilmSil(int id)
        {
            var film = _context.Filmlers.Find(id);
            if (film != null)
            {
                _context.Filmlers.Remove(film);
                _context.SaveChanges();
                _cache.Remove("filmList");
                TempData["Success"] = "Film başarıyla silindi.";
            }
            return RedirectToAction("Index");
        }

        // --- SALONLAR ---
        public IActionResult Salonlar(string searchString)
        {
            string cacheKey = "salonList";

            if (!_cache.TryGetValue(cacheKey, out List<Salonlar> salonlar))
            {
                salonlar = _context.Salonlars.ToList();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));

                _cache.Set(cacheKey, salonlar, cacheOptions);
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                salonlar = salonlar.Where(s => s.Ad != null && s.Ad.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            _logger.LogInformation("Toplam {Count} adet salon listelendi.", salonlar?.Count ?? 0);
            return View(salonlar);
        }

        public IActionResult ExportToPdf_Salonlar()
        {
            var salonlar = _context.Salonlars.ToList();
            var pdfDocument = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Arial"));

                    page.Header()
                        .Text("Salon Listesi Raporu")
                        .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                    page.Content()
                        .PaddingTop(1, Unit.Centimetre)
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(50);
                                columns.RelativeColumn();
                                columns.ConstantColumn(100);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("ID").Bold();
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Salon Adı").Bold();
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Kapasite").Bold();
                            });

                            foreach (var item in salonlar)
                            {
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.SalonId.ToString());
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.Ad ?? "");
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.Kapasite.ToString() + " Kişi");
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
            return File(pdfBytes, "application/pdf", $"Salon_Listesi_{DateTime.Now:yyyyMMdd}.pdf");
        }

        public IActionResult ExportToExcel_Salonlar()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var salonlar = _context.Salonlars.ToList();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Salon Listesi");

                worksheet.Cells[1, 1].Value = "Salon ID";
                worksheet.Cells[1, 2].Value = "Salon Adı";
                worksheet.Cells[1, 3].Value = "Kapasite";

                using (var range = worksheet.Cells[1, 1, 1, 3])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(41, 128, 185));
                    range.Style.Font.Color.SetColor(System.Drawing.Color.White);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                int rowNumber = 2;
                foreach (var item in salonlar)
                {
                    worksheet.Cells[rowNumber, 1].Value = item.SalonId;
                    worksheet.Cells[rowNumber, 2].Value = item.Ad;
                    worksheet.Cells[rowNumber, 3].Value = item.Kapasite;
                    rowNumber++;
                }

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                var fileBytes = package.GetAsByteArray();
                string fileName = $"Salon_Listesi_{DateTime.Now:yyyyMMdd}.xlsx";

                return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }

        [HttpGet]
        public IActionResult SalonEkle()
        {
            return View(new Salonlar());
        }

        [HttpPost]
        public IActionResult SalonEkle(Salonlar yeniSalon)
        {
            ModelState.Remove("Koltuklars");
            ModelState.Remove("Seanslars");
            if (ModelState.IsValid)
            {
                _context.Salonlars.Add(yeniSalon);
                _context.SaveChanges(); 

                int rows = (int)Math.Ceiling((double)yeniSalon.Kapasite / 10.0);
                int seatCount = 1;
                for (int r = 0; r < rows; r++)
                {
                    char siraChar = (char)('A' + r);
                    for (int n = 1; n <= 10; n++)
                    {
                        if (seatCount > yeniSalon.Kapasite) break;
                        _context.Koltuklars.Add(new Koltuklar
                        {
                            SalonId = yeniSalon.SalonId,
                            Sira = siraChar.ToString(),
                            Numara = n.ToString(),
                            DoluMu = false
                        });
                        seatCount++;
                    }
                }
                _context.SaveChanges();

                _cache.Remove("salonList");
                TempData["Success"] = "Salon ve koltuklar başarıyla oluşturuldu.";
                return RedirectToAction("Salonlar");
            }
            return View(yeniSalon);
        }

        [HttpGet]
        public IActionResult SalonDuzenle(int id)
        {
            var salon = _context.Salonlars.Find(id);
            if (salon == null) return NotFound();
            return View(salon);
        }

        [HttpPost]
        public IActionResult SalonDuzenle(Salonlar guncelSalon)
        {
            ModelState.Remove("Koltuklars");
            ModelState.Remove("Seanslars");
            if (ModelState.IsValid)
            {
                var salon = _context.Salonlars.Find(guncelSalon.SalonId);
                if (salon == null) return NotFound();

                salon.Ad = guncelSalon.Ad;
                salon.Kapasite = guncelSalon.Kapasite;
                // Not: Kapasite değiştiğinde koltukları otomatik yeniden ayarlamak karmaşık olabilir.
                // Basitlik adına sadece salon adını ve kapasite değerini güncelliyoruz.
                _context.SaveChanges();

                _cache.Remove("salonList");
                TempData["Success"] = "Salon başarıyla güncellendi.";
                return RedirectToAction("Salonlar");
            }
            return View(guncelSalon);
        }

        public IActionResult SalonSil(int id)
        {
            var salon = _context.Salonlars.Find(id);
            if (salon != null)
            {
                _context.Salonlars.Remove(salon);
                _context.SaveChanges();
                _cache.Remove("salonList");
                TempData["Success"] = "Salon silindi.";
            }
            return RedirectToAction("Salonlar");
        }

        // --- SEANSLAR ---
        public IActionResult Seanslar(string searchString)
        {
            string cacheKey = "seansList";

            if (!_cache.TryGetValue(cacheKey, out List<Seanslar> seanslar))
            {
                seanslar = _context.Seanslars.Include(s => s.Film).Include(s => s.Salon).ToList();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));

                _cache.Set(cacheKey, seanslar, cacheOptions);
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                seanslar = seanslar.Where(s => 
                    (s.Film?.Ad != null && s.Film.Ad.Contains(searchString, StringComparison.OrdinalIgnoreCase)) ||
                    (s.Salon?.Ad != null && s.Salon.Ad.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }

            _logger.LogInformation("Toplam {Count} adet seans listelendi.", seanslar?.Count ?? 0);
            return View(seanslar);
        }

        public IActionResult ExportToPdf_Seanslar()
        {
            var seanslar = _context.Seanslars.Include(s => s.Film).Include(s => s.Salon).ToList();
            var pdfDocument = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Arial"));

                    page.Header()
                        .Text("Seans Listesi Raporu")
                        .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                    page.Content()
                        .PaddingTop(1, Unit.Centimetre)
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(50);
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("ID").Bold();
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Film Adı").Bold();
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Salon Adı").Bold();
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Tarih ve Saat").Bold();
                            });

                            foreach (var item in seanslar)
                            {
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.SeansId.ToString());
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.Film?.Ad ?? "-");
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.Salon?.Ad ?? "-");
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.BaslamaZamani.ToString("dd.MM.yyyy HH:mm"));
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
            return File(pdfBytes, "application/pdf", $"Seans_Listesi_{DateTime.Now:yyyyMMdd}.pdf");
        }

        public IActionResult ExportToExcel_Seanslar()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var seanslar = _context.Seanslars.Include(s => s.Film).Include(s => s.Salon).ToList();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Seans Listesi");

                worksheet.Cells[1, 1].Value = "Seans ID";
                worksheet.Cells[1, 2].Value = "Film Adı";
                worksheet.Cells[1, 3].Value = "Salon Adı";
                worksheet.Cells[1, 4].Value = "Tarih ve Saat";

                using (var range = worksheet.Cells[1, 1, 1, 4])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(41, 128, 185));
                    range.Style.Font.Color.SetColor(System.Drawing.Color.White);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                int rowNumber = 2;
                foreach (var item in seanslar)
                {
                    worksheet.Cells[rowNumber, 1].Value = item.SeansId;
                    worksheet.Cells[rowNumber, 2].Value = item.Film?.Ad;
                    worksheet.Cells[rowNumber, 3].Value = item.Salon?.Ad;
                    worksheet.Cells[rowNumber, 4].Value = item.BaslamaZamani.ToString("dd.MM.yyyy HH:mm");
                    rowNumber++;
                }

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                var fileBytes = package.GetAsByteArray();
                string fileName = $"Seans_Listesi_{DateTime.Now:yyyyMMdd}.xlsx";

                return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }

        [HttpGet]
        public IActionResult SeansEkle()
        {
            ViewBag.Filmler = _context.Filmlers.ToList();
            ViewBag.Salonlar = _context.Salonlars.ToList();
            return View(new Seanslar { BaslamaZamani = DateTime.Now });
        }

        [HttpPost]
        public IActionResult SeansEkle(int FilmId, int SalonId, DateTime BaslamaZamani)
        {
            if (BaslamaZamani.Year < 2000)
            {
                TempData["Error"] = "Lütfen geçerli bir tarih seçiniz.";
                return RedirectToAction("SeansEkle");
            }

            var yeniSeans = new Seanslar
            {
                FilmId = FilmId,
                SalonId = SalonId,
                BaslamaZamani = BaslamaZamani
            };

            _context.Seanslars.Add(yeniSeans);
            _context.SaveChanges();
            _cache.Remove("seansList");
            TempData["Success"] = "Seans başarıyla eklendi.";
            return RedirectToAction("Seanslar");
        }

        [HttpGet]
        public IActionResult SeansDuzenle(int id)
        {
            var seans = _context.Seanslars.Find(id);
            if (seans == null) return NotFound();

            ViewBag.Filmler = _context.Filmlers.ToList();
            ViewBag.Salonlar = _context.Salonlars.ToList();
            return View(seans);
        }

        [HttpPost]
        public IActionResult SeansDuzenle(int SeansId, int FilmId, int SalonId, DateTime BaslamaZamani)
        {
            if (BaslamaZamani.Year < 2000)
            {
                TempData["Error"] = "Lütfen geçerli bir tarih seçiniz.";
                return RedirectToAction("SeansDuzenle", new { id = SeansId });
            }

            var seans = _context.Seanslars.Find(SeansId);
            if (seans == null) return NotFound();

            seans.FilmId = FilmId;
            seans.SalonId = SalonId;
            seans.BaslamaZamani = BaslamaZamani;

            _context.SaveChanges();
            _cache.Remove("seansList");
            TempData["Success"] = "Seans başarıyla güncellendi.";
            return RedirectToAction("Seanslar");
        }

        public IActionResult SeansSil(int id)
        {
            var seans = _context.Seanslars.Find(id);
            if (seans != null)
            {
                _context.Seanslars.Remove(seans);
                _context.SaveChanges();
                _cache.Remove("seansList");
                TempData["Success"] = "Seans silindi.";
            }
            return RedirectToAction("Seanslar");
        }

        // --- RAPORLAR ---
        [HttpGet]
        public IActionResult Raporlar()
        {
            var model = new RaporViewModel();

            // 1. Film Satis Raporu
            model.FilmSatisRaporlari = _context.Filmlers
                .Select(f => new FilmSatisRaporu
                {
                    FilmAdi = f.Ad ?? "Bilinmiyor",
                    SatilanBiletSayisi = _context.Biletlers.Count(b => b.Seans != null && b.Seans.FilmId == f.FilmId),
                    ToplamHasilat = _context.Biletlers.Where(b => b.Seans != null && b.Seans.FilmId == f.FilmId).Sum(b => (decimal?)b.Fiyat) ?? 0
                }).ToList();

            // 2. Seans Doluluk Raporu
            model.SeansDolulukRaporlari = _context.Seanslars
                .Select(s => new SeansDolulukRaporu
                {
                    TarihSaat = s.BaslamaZamani,
                    FilmAdi = s.Film != null ? s.Film.Ad ?? "Bilinmiyor" : "Bilinmiyor",
                    SalonAdi = s.Salon != null ? s.Salon.Ad ?? "Bilinmiyor" : "Bilinmiyor",
                    SalonKapasitesi = s.Salon != null ? s.Salon.Kapasite : 0,
                    SatilanKoltukSayisi = _context.Biletlers.Count(b => b.SeansId == s.SeansId)
                }).ToList();

            // 3. Kullanici Harcama Raporu
            model.KullaniciHarcamaRaporlari = _context.Users
                .Select(u => new KullaniciHarcamaRaporu
                {
                    UserId = u.Id,
                    KullaniciEmail = u.Email ?? "Bilinmiyor",
                    ToplamBiletSayisi = _context.Biletlers.Count(b => b.UserId == u.Id),
                    ToplamHarcama = _context.Biletlers.Where(b => b.UserId == u.Id).Sum(b => (decimal?)b.Fiyat) ?? 0
                })
                .Where(x => x.ToplamBiletSayisi > 0)
                .ToList();

            // 4. Tur Hasilat Raporu
            model.TurHasilatRaporlari = _context.Filmlers
                .Where(f => f.Tur != null)
                .GroupBy(f => f.Tur)
                .Select(g => new TurHasilatRaporu
                {
                    FilmTuru = g.Key ?? "Bilinmiyor",
                    ToplamHasilat = _context.Biletlers.Where(b => b.Seans != null && b.Seans.Film != null && b.Seans.Film.Tur == g.Key).Sum(b => (decimal?)b.Fiyat) ?? 0
                }).ToList();

            // 5. Salon Satis Raporu
            model.SalonSatisRaporlari = _context.Salonlars
                .Select(s => new SalonSatisRaporu
                {
                    SalonAdi = s.Ad ?? "Bilinmiyor",
                    ToplamSatilanBilet = _context.Biletlers.Count(b => b.Seans != null && b.Seans.SalonId == s.SalonId),
                    ToplamGelir = _context.Biletlers.Where(b => b.Seans != null && b.Seans.SalonId == s.SalonId).Sum(b => (decimal?)b.Fiyat) ?? 0
                }).ToList();

            return View(model);
        }
    }
}
