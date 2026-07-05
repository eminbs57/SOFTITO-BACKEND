using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Collections.Generic;
using MvcProject.Models;
using MvcProject.Services;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace MvcProject.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly ApiService<Reservation> _apiService;
        private readonly ApiService<Guest> _guestService;
        private readonly ApiService<Room> _roomService;
        private readonly ApiService<RoomType> _roomTypeService;

        public ReservationsController(
            ApiService<Reservation> apiService,
            ApiService<Guest> guestService,
            ApiService<Room> roomService,
            ApiService<RoomType> roomTypeService)
        {
            _apiService = apiService;
            _guestService = guestService;
            _roomService = roomService;
            _roomTypeService = roomTypeService;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            var data = await _apiService.GetAllAsync();
            
            if (!string.IsNullOrEmpty(searchString))
            {
                data = data.Where(s => 
                    (s.FirstName != null && s.FirstName.Contains(searchString, StringComparison.OrdinalIgnoreCase)) ||
                    (s.LastName != null && s.LastName.Contains(searchString, StringComparison.OrdinalIgnoreCase)) ||
                    (s.RoomNumber != null && s.RoomNumber.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                );
            }

            ViewData["CurrentFilter"] = searchString;
            return View(data);
        }

        public async Task<IActionResult> Create()
        {
            await PopulateViewBagsForReservation();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Reservation entity)
        {
            if (entity.CheckInDate >= entity.CheckOutDate)
            {
                TempData["ErrorMessage"] = "Çıkış tarihi giriş tarihinden sonra olmalıdır.";
                await PopulateViewBagsForReservation(entity.GuestId, entity.RoomId);
                return View(entity);
            }

            var allReservations = await _apiService.GetAllAsync();
            var isOverlapping = allReservations.Any(r => 
                r.RoomId == entity.RoomId && 
                entity.CheckInDate < r.CheckOutDate && 
                entity.CheckOutDate > r.CheckInDate);

            if (isOverlapping)
            {
                TempData["ErrorMessage"] = "Seçilen tarihler arasında bu oda zaten rezerve edilmiş. Lütfen başka bir tarih veya oda seçin.";
                await PopulateViewBagsForReservation(entity.GuestId, entity.RoomId);
                return View(entity);
            }

            await _apiService.CreateAsync(entity);

            // Update room availability automatically
            var room = await _roomService.GetByIdAsync(entity.RoomId);
            if (room != null)
            {
                room.IsAvailable = false;
                await _roomService.UpdateAsync(room.Id, room);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var data = await _apiService.GetByIdAsync(id);
            if (data == null) return NotFound();

            await PopulateViewBagsForReservation(data.GuestId, data.RoomId);

            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Reservation entity)
        {
            if (entity.CheckInDate >= entity.CheckOutDate)
            {
                TempData["ErrorMessage"] = "Çıkış tarihi giriş tarihinden sonra olmalıdır.";
                await PopulateViewBagsForReservation(entity.GuestId, entity.RoomId);
                return View(entity);
            }

            var allReservations = await _apiService.GetAllAsync();
            var isOverlapping = allReservations.Any(r => 
                r.Id != id &&
                r.RoomId == entity.RoomId && 
                entity.CheckInDate < r.CheckOutDate && 
                entity.CheckOutDate > r.CheckInDate);

            if (isOverlapping)
            {
                TempData["ErrorMessage"] = "Seçilen tarihler arasında bu oda zaten rezerve edilmiş. Lütfen başka bir tarih veya oda seçin.";
                await PopulateViewBagsForReservation(entity.GuestId, entity.RoomId);
                return View(entity);
            }

            await _apiService.UpdateAsync(id, entity);
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateViewBagsForReservation(int? selectedGuestId = null, int? selectedRoomId = null)
        {
            var guests = await _guestService.GetAllAsync();
            var guestList = guests.Select(g => new { Id = g.Id, FullName = g.FirstName + " " + g.LastName });
            ViewBag.Guests = new SelectList(guestList, "Id", "FullName", selectedGuestId);

            var rooms = await _roomService.GetAllAsync();
            ViewBag.Rooms = new SelectList(rooms, "Id", "RoomNumber", selectedRoomId);
            
            var roomTypes = await _roomTypeService.GetAllAsync();
            var roomPrices = rooms.Select(r => new { 
                RoomId = r.Id, 
                Price = roomTypes.FirstOrDefault(rt => rt.Id == r.RoomTypeId)?.BasePrice ?? 0 
            });
            ViewBag.RoomPrices = System.Text.Json.JsonSerializer.Serialize(roomPrices);
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _apiService.DeleteAsync(id);
            }
            catch (System.Exception)
            {
                TempData["ErrorMessage"] = "Bu kayıt kullanımda olduğu için silinemez.";
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ExportToPdf()
        {
            var reservations = (await _apiService.GetAllAsync()).ToList();

            var pdfDocument = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Arial"));

                    page.Header()
                        .Text("Rezervasyon Listesi Raporu")
                        .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                    page.Content()
                        .PaddingTop(1, Unit.Centimetre)
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(50);  
                                columns.RelativeColumn();    
                                columns.ConstantColumn(80); 
                                columns.ConstantColumn(80); 
                                columns.ConstantColumn(80); 
                            });

                            table.Header(header =>
                            {
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("ID").Bold();
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Misafir").Bold();
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Oda No").Bold();
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Giriş").Bold();
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Çıkış").Bold();
                            });

                            foreach (var item in reservations)
                            {
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.Id.ToString());
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.FirstName + " " + item.LastName);
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.RoomNumber.ToString());
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.CheckInDate.ToShortDateString());
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.CheckOutDate.ToShortDateString());
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
            return File(pdfBytes, "application/pdf", $"Rezervasyonlar_{DateTime.Now:yyyyMMdd}.pdf");
        }

        public async Task<IActionResult> ExportToExcel()
        {
            ExcelPackage.License.SetNonCommercialPersonal("Backend softito");
            var reservations = (await _apiService.GetAllAsync()).ToList();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Rezervasyon Listesi");

                worksheet.Cells[1, 1].Value = "Rezervasyon ID";
                worksheet.Cells[1, 2].Value = "Misafir Adı";
                worksheet.Cells[1, 3].Value = "Oda Numarası";
                worksheet.Cells[1, 4].Value = "Giriş Tarihi";
                worksheet.Cells[1, 5].Value = "Çıkış Tarihi";
                worksheet.Cells[1, 6].Value = "Toplam Fiyat";

                using (var range = worksheet.Cells[1, 1, 1, 6]) 
                {
                    range.Style.Font.Bold = true; 
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(41, 128, 185)); 
                    range.Style.Font.Color.SetColor(System.Drawing.Color.White); 
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; 
                }

                int rowNumber = 2; 
                foreach (var item in reservations)
                {
                    worksheet.Cells[rowNumber, 1].Value = item.Id;
                    worksheet.Cells[rowNumber, 2].Value = item.FirstName + " " + item.LastName;
                    worksheet.Cells[rowNumber, 3].Value = item.RoomNumber;
                    worksheet.Cells[rowNumber, 4].Value = item.CheckInDate.ToShortDateString();
                    worksheet.Cells[rowNumber, 5].Value = item.CheckOutDate.ToShortDateString();
                    worksheet.Cells[rowNumber, 6].Value = item.TotalPrice;
                    rowNumber++;
                }

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                var fileBytes = package.GetAsByteArray();
                string fileName = $"Rezervasyonlar_{DateTime.Now:yyyyMMdd}.xlsx";

                return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }
    }
}
