using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Text.Json;
using System.Collections.Generic;
using ObiletApp.Core.Interfaces;

namespace ObiletApp.Web.Controllers
{
    public class BookingController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IQrCodeService _qrCodeService;

        public BookingController(IHttpClientFactory httpClientFactory, IQrCodeService qrCodeService)
        {
            _httpClient = httpClientFactory.CreateClient();
            _qrCodeService = qrCodeService;
        }

        [HttpPost]
        public async Task<IActionResult> Search(int originId, int destinationId, string date)
        {
            try
            {
                var response = await _httpClient.GetStringAsync($"http://localhost:5028/api/TripQuery/Search?originId={originId}&destinationId={destinationId}&date={date}");
                var trips = JsonSerializer.Deserialize<List<dynamic>>(response);

                ViewBag.Trips = trips;
                ViewBag.Date = date;
                return View();
            }
            catch
            {
                ViewBag.Trips = new List<dynamic>();
                return View();
            }
        }

        public async Task<IActionResult> SeatSelection(int tripId, string date)
        {
            try
            {
                var response = await _httpClient.GetStringAsync($"http://localhost:5028/api/TripQuery/{tripId}/OccupiedSeats");
                var occupiedSeats = JsonSerializer.Deserialize<List<string>>(response);

                ViewBag.TripId = tripId;
                ViewBag.OccupiedSeats = occupiedSeats;
                ViewBag.Date = date;
                return View();
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(int tripId, string seatNumber, string passengerName)
        {
            string pnr = GeneratePNR();

            var newTicket = new {
                TripId = tripId,
                PassengerId = 1, 
                UserId = passengerName, 
                SeatNumber = seatNumber,
                PNR = pnr,
                Status = 1 
            };

            try 
            {
                var content = new StringContent(JsonSerializer.Serialize(newTicket), System.Text.Encoding.UTF8, "application/json");
                await _httpClient.PostAsync("http://localhost:5108/api/Ticket", content);
            }
            catch {}

            return RedirectToAction("Success", new { pnr = pnr, name = passengerName, seat = seatNumber });
        }

        public IActionResult Success(string pnr, string name, string seat)
        {
            ViewBag.PNR = pnr;
            ViewBag.PassengerName = name;
            ViewBag.SeatNumber = seat;
            
            // Bilet bilgilerini içeren karekodu (QR) üret
            string qrText = $"PNR: {pnr} | Yolcu: {name} | Koltuk: {seat}";
            ViewBag.QrCodeImage = _qrCodeService.GenerateQrCodeBase64(qrText);
            
            return View();
        }

        [HttpGet]
        public IActionResult FindTicket()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> FindTicket(string pnr, string passengerName)
        {
            if (string.IsNullOrEmpty(pnr) || string.IsNullOrEmpty(passengerName))
            {
                ViewBag.Error = "Lütfen PNR kodu ve Ad Soyad bilgilerini eksiksiz giriniz.";
                return View();
            }

            try
            {
                var response = await _httpClient.GetAsync($"http://localhost:5028/api/TicketQuery/Pnr/{pnr}?name={passengerName}");

                if (response.IsSuccessStatusCode)
                {
                    var ticketStr = await response.Content.ReadAsStringAsync();
                    var ticket = JsonSerializer.Deserialize<dynamic>(ticketStr);
                    return View("TicketDetail", ticket);
                }
                else
                {
                    ViewBag.Error = "Girdiğiniz bilgilere ait bilet bulunamadı veya bilgiler eşleşmiyor.";
                    return View();
                }
            }
            catch
            {
                ViewBag.Error = "Sistemde bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.";
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> CancelTicket(int ticketId)
        {
            try
            {
                await _httpClient.PutAsync($"http://localhost:5108/api/Ticket/{ticketId}/cancel", null);
            }
            catch {}

            return RedirectToAction("FindTicket");
        }

        private string GeneratePNR()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var pnr = new char[6];
            for (int i = 0; i < 6; i++)
            {
                pnr[i] = chars[random.Next(chars.Length)];
            }
            return new string(pnr);
        }
    }
}
