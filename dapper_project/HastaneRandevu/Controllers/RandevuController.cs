using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using Dapper;
using HastaneRandevu.Models;
using ClosedXML.Excel;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace HastaneRandevu.Controllers
{
    [Authorize]
    public class RandevuController : Controller
    {
        private readonly ILogger<RandevuController> _logger;
        private readonly IMemoryCache _cache;

        public RandevuController(ILogger<RandevuController> logger, IMemoryCache cache)
        {
            _logger = logger;
            _cache = cache;
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public IActionResult Index(string searchTerm = "")
        {
            ViewBag.Hastalar = Context.Listeleme<HastaModel>("HastaViewAll") ?? new List<HastaModel>();
            ViewBag.Doktorlar = Context.Listeleme<DoktorModel>("DoktorViewAll") ?? new List<DoktorModel>();

            string cacheKey = "randevuList";

            if (!_cache.TryGetValue(cacheKey, out List<RandevuModel> randevular))
            {
                randevular = Context.Listeleme<RandevuModel>("RandevuViewAll").ToList();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));

                _cache.Set(cacheKey, randevular, cacheOptions);
            }

            _logger.LogInformation("Toplam {Count} adet randevu veri tabanından/cache'ten listelendi.", randevular.Count);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                randevular = randevular.Where(r => r.Durum != null && r.Durum.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            ViewBag.SearchTerm = searchTerm;
            return View(randevular);
        }

        public IActionResult EY(int id = 0)
        {
            ViewBag.Hastalar = Context.Listeleme<HastaModel>("HastaViewAll") ?? new List<HastaModel>();
            ViewBag.Poliklinikler = Context.Listeleme<PoliklinikModel>("PoliklinikViewAll") ?? new List<PoliklinikModel>();
            ViewBag.Doktorlar = Context.Listeleme<DoktorModel>("DoktorViewAll") ?? new List<DoktorModel>();
            if (id == 0)
            {
                return View();
            }
            else
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@RandevuNo", id);
                return View(Context.Listeleme<RandevuModel>("RandevuViewByNo", param).FirstOrDefault());
            }
        }

        [HttpPost]
        public IActionResult EY(RandevuModel randevu)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@RandevuNo", randevu.RandevuNo);
            param.Add("@HastaNo", randevu.HastaNo);
            param.Add("@DoktorNo", randevu.DoktorNo);
            param.Add("@RandevuTarihi", randevu.RandevuTarihi);
            param.Add("@Durum", randevu.Durum ?? "Aktif");
            Context.ExecuteReturn("RandevuEY", param);
            
            _cache.Remove("randevuList"); // Invalidate cache
            
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id = 0)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@RandevuNo", id);
            Context.ExecuteReturn("RandevuSil", param);
            
            _cache.Remove("randevuList"); // Invalidate cache
            
            return RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult PoliklinikDoktorlariniGetir(int poliklinikNo)
        {
            var doktorlar = Context.Listeleme<DoktorModel>("DoktorViewAll")
                                   .Where(d => d.PoliklinikNo == poliklinikNo)
                                   .ToList();
            return Json(doktorlar);
        }

        public IActionResult DownloadExcel()
        {
            var randevular = Context.Listeleme<RandevuModel>("RandevuViewAll").ToList();
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Randevular");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "No";
                worksheet.Cell(currentRow, 2).Value = "Hasta No";
                worksheet.Cell(currentRow, 3).Value = "Doktor No";
                worksheet.Cell(currentRow, 4).Value = "Tarih";
                worksheet.Cell(currentRow, 5).Value = "Durum";

                foreach (var r in randevular)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = r.RandevuNo;
                    worksheet.Cell(currentRow, 2).Value = r.HastaNo;
                    worksheet.Cell(currentRow, 3).Value = r.DoktorNo;
                    worksheet.Cell(currentRow, 4).Value = r.RandevuTarihi.ToString("dd.MM.yyyy HH:mm");
                    worksheet.Cell(currentRow, 5).Value = r.Durum;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Randevular.xlsx");
                }
            }
        }

        public IActionResult DownloadPdf()
        {
            var randevular = Context.Listeleme<RandevuModel>("RandevuViewAll").ToList();
            
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(11));

                    page.Header().Text("Randevu Listesi")
                        .SemiBold().FontSize(20).FontColor(Colors.Blue.Darken2);

                    page.Content().PaddingVertical(1, Unit.Centimetre).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        table.Header(header =>
                        {
                            header.Cell().Text("No").SemiBold();
                            header.Cell().Text("Hasta No").SemiBold();
                            header.Cell().Text("Doktor No").SemiBold();
                            header.Cell().Text("Tarih").SemiBold();
                            header.Cell().Text("Durum").SemiBold();
                        });

                        foreach (var item in randevular)
                        {
                            table.Cell().Text(item.RandevuNo.ToString());
                            table.Cell().Text(item.HastaNo.ToString());
                            table.Cell().Text(item.DoktorNo.ToString());
                            table.Cell().Text(item.RandevuTarihi.ToString("dd.MM.yyyy HH:mm"));
                            table.Cell().Text(item.Durum ?? "");
                        }
                    });
                });
            });

            byte[] pdfBytes = document.GeneratePdf();
            return File(pdfBytes, "application/pdf", "Randevular.pdf");
        }
    }
}