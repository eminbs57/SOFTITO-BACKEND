using Microsoft.AspNetCore.Mvc;
using ObiletApp.Infrastructure.Contexts;
using ObiletApp.Core.Entities;

namespace ObiletApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        private readonly ObiletDbContext _context;

        public SeedController(ObiletDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult SeedData()
        {

            if (!_context.Set<Company>().Any())
            {
                var companies = new List<Company>
                {
                    new Company { Name = "Kamil Koç", LogoUrl = "https://example.com/logo1.png", IsActive = true, CreatedDate = DateTime.Now },
                    new Company { Name = "Metro Turizm", LogoUrl = "https://example.com/logo2.png", IsActive = true, CreatedDate = DateTime.Now },
                    new Company { Name = "Pamukkale Turizm", LogoUrl = "https://example.com/logo3.png", IsActive = true, CreatedDate = DateTime.Now },
                    new Company { Name = "Ali Osman Ulusoy", LogoUrl = "https://example.com/logo4.png", IsActive = true, CreatedDate = DateTime.Now },
                    new Company { Name = "Varan Turizm", LogoUrl = "https://example.com/logo5.png", IsActive = true, CreatedDate = DateTime.Now },
                    new Company { Name = "Lüks Karadeniz", LogoUrl = "https://example.com/logo6.png", IsActive = true, CreatedDate = DateTime.Now }
                };
                _context.Set<Company>().AddRange(companies);
                _context.SaveChanges();
            }

            if (!_context.Set<Location>().Any())
            {
                var locations = new List<Location>
                {
                    new Location { CityName = "İstanbul", TerminalName = "Esenler Otogarı", IsActive = true, CreatedDate = DateTime.Now },
                    new Location { CityName = "Ankara", TerminalName = "AŞTİ", IsActive = true, CreatedDate = DateTime.Now },
                    new Location { CityName = "İzmir", TerminalName = "İzotaş", IsActive = true, CreatedDate = DateTime.Now },
                    new Location { CityName = "Bursa", TerminalName = "Bursa Terminali", IsActive = true, CreatedDate = DateTime.Now },
                    new Location { CityName = "Antalya", TerminalName = "Antalya Otogarı", IsActive = true, CreatedDate = DateTime.Now },
                    new Location { CityName = "Trabzon", TerminalName = "Trabzon Şehirlerarası Otobüs Terminali", IsActive = true, CreatedDate = DateTime.Now }
                };
                _context.Set<Location>().AddRange(locations);
                _context.SaveChanges();
            }

            var companyIds = _context.Set<Company>().Select(c => c.Id).ToList();
            var locationIds = _context.Set<Location>().Select(l => l.Id).ToList();

            if (!_context.Set<Vehicle>().Any() && companyIds.Count > 0)
            {
                var vehicles = new List<Vehicle>
                {
                    new Vehicle { CompanyId = companyIds[0], Type = ObiletApp.Core.Enums.VehicleType.Bus, Capacity = 41, Plate = "34 ABC 123", IsActive = true, CreatedDate = DateTime.Now },
                    new Vehicle { CompanyId = companyIds[1], Type = ObiletApp.Core.Enums.VehicleType.Bus, Capacity = 54, Plate = "06 DEF 456", IsActive = true, CreatedDate = DateTime.Now },
                    new Vehicle { CompanyId = companyIds[2], Type = ObiletApp.Core.Enums.VehicleType.Bus, Capacity = 41, Plate = "35 GHI 789", IsActive = true, CreatedDate = DateTime.Now },
                    new Vehicle { CompanyId = companyIds[3], Type = ObiletApp.Core.Enums.VehicleType.Bus, Capacity = 41, Plate = "61 JKL 012", IsActive = true, CreatedDate = DateTime.Now },
                    new Vehicle { CompanyId = companyIds[4], Type = ObiletApp.Core.Enums.VehicleType.Bus, Capacity = 54, Plate = "34 MNO 345", IsActive = true, CreatedDate = DateTime.Now },
                    new Vehicle { CompanyId = companyIds[0], Type = ObiletApp.Core.Enums.VehicleType.Bus, Capacity = 54, Plate = "16 PQR 678", IsActive = true, CreatedDate = DateTime.Now }
                };
                _context.Set<Vehicle>().AddRange(vehicles);
                _context.SaveChanges();
            }

            if (!_context.Set<ObiletApp.Core.Entities.Route>().Any() && locationIds.Count > 1)
            {
                var routes = new List<ObiletApp.Core.Entities.Route>
                {
                    new ObiletApp.Core.Entities.Route { DepartureLocationId = locationIds[0], ArrivalLocationId = locationIds[1], DistanceInKm = 450, IsActive = true, CreatedDate = DateTime.Now },
                    new ObiletApp.Core.Entities.Route { DepartureLocationId = locationIds[1], ArrivalLocationId = locationIds[0], DistanceInKm = 450, IsActive = true, CreatedDate = DateTime.Now },
                    new ObiletApp.Core.Entities.Route { DepartureLocationId = locationIds[0], ArrivalLocationId = locationIds[2], DistanceInKm = 480, IsActive = true, CreatedDate = DateTime.Now },
                    new ObiletApp.Core.Entities.Route { DepartureLocationId = locationIds[2], ArrivalLocationId = locationIds[0], DistanceInKm = 480, IsActive = true, CreatedDate = DateTime.Now },
                    new ObiletApp.Core.Entities.Route { DepartureLocationId = locationIds[1], ArrivalLocationId = locationIds[2], DistanceInKm = 580, IsActive = true, CreatedDate = DateTime.Now },
                    new ObiletApp.Core.Entities.Route { DepartureLocationId = locationIds[3], ArrivalLocationId = locationIds[4], DistanceInKm = 550, IsActive = true, CreatedDate = DateTime.Now }
                };
                _context.Set<ObiletApp.Core.Entities.Route>().AddRange(routes);
                _context.SaveChanges();
            }

            var vehicleIds = _context.Set<Vehicle>().Select(v => v.Id).ToList();
            var routeIds = _context.Set<ObiletApp.Core.Entities.Route>().Select(r => r.Id).ToList();

            if (!_context.Set<Trip>().Any() && routeIds.Count > 0 && vehicleIds.Count > 0)
            {
                var trips = new List<Trip>
                {
                    new Trip { RouteId = routeIds[0], VehicleId = vehicleIds[0], DepartureTime = DateTime.Now.AddDays(1).AddHours(10), ArrivalTime = DateTime.Now.AddDays(1).AddHours(16), Price = 450.0m, IsActive = true, CreatedDate = DateTime.Now },
                    new Trip { RouteId = routeIds[1], VehicleId = vehicleIds[1], DepartureTime = DateTime.Now.AddDays(1).AddHours(12), ArrivalTime = DateTime.Now.AddDays(1).AddHours(18), Price = 400.0m, IsActive = true, CreatedDate = DateTime.Now },
                    new Trip { RouteId = routeIds[2], VehicleId = vehicleIds[2], DepartureTime = DateTime.Now.AddDays(2).AddHours(8), ArrivalTime = DateTime.Now.AddDays(2).AddHours(15), Price = 500.0m, IsActive = true, CreatedDate = DateTime.Now },
                    new Trip { RouteId = routeIds[3], VehicleId = vehicleIds[3], DepartureTime = DateTime.Now.AddDays(2).AddHours(22), ArrivalTime = DateTime.Now.AddDays(3).AddHours(5), Price = 550.0m, IsActive = true, CreatedDate = DateTime.Now },
                    new Trip { RouteId = routeIds[4], VehicleId = vehicleIds[4], DepartureTime = DateTime.Now.AddDays(3).AddHours(14), ArrivalTime = DateTime.Now.AddDays(3).AddHours(22), Price = 350.0m, IsActive = true, CreatedDate = DateTime.Now },
                    new Trip { RouteId = routeIds[5], VehicleId = vehicleIds[5], DepartureTime = DateTime.Now.AddDays(4).AddHours(9), ArrivalTime = DateTime.Now.AddDays(4).AddHours(17), Price = 600.0m, IsActive = true, CreatedDate = DateTime.Now }
                };
                _context.Set<Trip>().AddRange(trips);
                _context.SaveChanges();
            }

            if (!_context.Set<Passenger>().Any())
            {
                var passengers = new List<Passenger>
                {
                    new Passenger { FirstName = "Ahmet", LastName = "Yılmaz", IdentityNumber = "12345678901", Gender = ObiletApp.Core.Enums.Gender.Male, IsActive = true, CreatedDate = DateTime.Now },
                    new Passenger { FirstName = "Ayşe", LastName = "Kaya", IdentityNumber = "23456789012", Gender = ObiletApp.Core.Enums.Gender.Female, IsActive = true, CreatedDate = DateTime.Now },
                    new Passenger { FirstName = "Mehmet", LastName = "Demir", IdentityNumber = "34567890123", Gender = ObiletApp.Core.Enums.Gender.Male, IsActive = true, CreatedDate = DateTime.Now },
                    new Passenger { FirstName = "Fatma", LastName = "Çelik", IdentityNumber = "45678901234", Gender = ObiletApp.Core.Enums.Gender.Female, IsActive = true, CreatedDate = DateTime.Now },
                    new Passenger { FirstName = "Mustafa", LastName = "Şahin", IdentityNumber = "56789012345", Gender = ObiletApp.Core.Enums.Gender.Male, IsActive = true, CreatedDate = DateTime.Now },
                    new Passenger { FirstName = "Zeynep", LastName = "Öztürk", IdentityNumber = "67890123456", Gender = ObiletApp.Core.Enums.Gender.Female, IsActive = true, CreatedDate = DateTime.Now }
                };
                _context.Set<Passenger>().AddRange(passengers);
                _context.SaveChanges();
            }

            var tripIds = _context.Set<Trip>().Select(t => t.Id).ToList();
            var passengerIds = _context.Set<Passenger>().Select(p => p.Id).ToList();

            if (!_context.Set<Campaign>().Any() && companyIds.Count > 0)
            {
                var campaigns = new List<Campaign>
                {
                    new Campaign { CompanyId = companyIds[0], Name = "Yaz İndirimi", DiscountCode = "YAZ2024", DiscountPercentage = 15.0m, StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(2), IsActive = true, CreatedDate = DateTime.Now },
                    new Campaign { CompanyId = companyIds[1], Name = "Öğrenci Kampanyası", DiscountCode = "OGRENCI20", DiscountPercentage = 20.0m, StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(6), IsActive = true, CreatedDate = DateTime.Now },
                    new Campaign { CompanyId = companyIds[2], Name = "Erken Rezervasyon", DiscountCode = "ERKEN10", DiscountPercentage = 10.0m, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(15), IsActive = true, CreatedDate = DateTime.Now },
                    new Campaign { CompanyId = companyIds[3], Name = "Gidiş-Dönüş İndirimi", DiscountCode = "GIDISDONUS", DiscountPercentage = 25.0m, StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1), IsActive = true, CreatedDate = DateTime.Now },
                    new Campaign { CompanyId = companyIds[4], Name = "Yeni Üye İndirimi", DiscountCode = "HOSGELDIN", DiscountPercentage = 30.0m, StartDate = DateTime.Now, EndDate = DateTime.Now.AddYears(1), IsActive = true, CreatedDate = DateTime.Now },
                    new Campaign { CompanyId = companyIds[0], Name = "Haftasonu Kaçamağı", DiscountCode = "HAFTASONU", DiscountPercentage = 12.0m, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(30), IsActive = true, CreatedDate = DateTime.Now }
                };
                _context.Set<Campaign>().AddRange(campaigns);
                _context.SaveChanges();
            }

            if (!_context.Set<Ticket>().Any() && tripIds.Count > 0 && passengerIds.Count > 0)
            {
                var tickets = new List<Ticket>
                {
                    new Ticket { TripId = tripIds[0], PassengerId = passengerIds[0], SeatNumber = "12", PNR = "PNR12345", Status = ObiletApp.Core.Enums.TicketStatus.Active, IsActive = true, CreatedDate = DateTime.Now },
                    new Ticket { TripId = tripIds[0], PassengerId = passengerIds[1], SeatNumber = "13", PNR = "PNR23456", Status = ObiletApp.Core.Enums.TicketStatus.Active, IsActive = true, CreatedDate = DateTime.Now },
                    new Ticket { TripId = tripIds[1], PassengerId = passengerIds[2], SeatNumber = "5", PNR = "PNR34567", Status = ObiletApp.Core.Enums.TicketStatus.Cancelled, IsActive = false, CreatedDate = DateTime.Now },
                    new Ticket { TripId = tripIds[2], PassengerId = passengerIds[3], SeatNumber = "21", PNR = "PNR45678", Status = ObiletApp.Core.Enums.TicketStatus.Active, IsActive = true, CreatedDate = DateTime.Now },
                    new Ticket { TripId = tripIds[3], PassengerId = passengerIds[4], SeatNumber = "8", PNR = "PNR56789", Status = ObiletApp.Core.Enums.TicketStatus.Active, IsActive = true, CreatedDate = DateTime.Now },
                    new Ticket { TripId = tripIds[4], PassengerId = passengerIds[5], SeatNumber = "15", PNR = "PNR67890", Status = ObiletApp.Core.Enums.TicketStatus.Active, IsActive = true, CreatedDate = DateTime.Now }
                };
                _context.Set<Ticket>().AddRange(tickets);
                _context.SaveChanges();

                var ticketIds = _context.Set<Ticket>().Select(t => t.Id).ToList();

                var payments = new List<Payment>
                {
                    new Payment { TicketId = ticketIds[0], Amount = 450.0m, Method = ObiletApp.Core.Enums.PaymentMethod.CreditCard, IsSuccessful = true, IsActive = true, CreatedDate = DateTime.Now },
                    new Payment { TicketId = ticketIds[1], Amount = 450.0m, Method = ObiletApp.Core.Enums.PaymentMethod.CreditCard, IsSuccessful = true, IsActive = true, CreatedDate = DateTime.Now },
                    new Payment { TicketId = ticketIds[2], Amount = 400.0m, Method = ObiletApp.Core.Enums.PaymentMethod.CreditCard, IsSuccessful = false, IsActive = false, CreatedDate = DateTime.Now },
                    new Payment { TicketId = ticketIds[3], Amount = 500.0m, Method = ObiletApp.Core.Enums.PaymentMethod.CreditCard, IsSuccessful = true, IsActive = true, CreatedDate = DateTime.Now },
                    new Payment { TicketId = ticketIds[4], Amount = 550.0m, Method = ObiletApp.Core.Enums.PaymentMethod.CreditCard, IsSuccessful = true, IsActive = true, CreatedDate = DateTime.Now },
                    new Payment { TicketId = ticketIds[5], Amount = 350.0m, Method = ObiletApp.Core.Enums.PaymentMethod.CreditCard, IsSuccessful = true, IsActive = true, CreatedDate = DateTime.Now }
                };
                _context.Set<Payment>().AddRange(payments);

                var reviews = new List<Review>
                {
                    new Review { TripId = tripIds[0], Rating = 5, Comment = "Şoför çok dikkatliydi, araç temizdi.", IsActive = true, CreatedDate = DateTime.Now },
                    new Review { TripId = tripIds[1], Rating = 3, Comment = "Klima biraz fazla soğuktu.", IsActive = true, CreatedDate = DateTime.Now },
                    new Review { TripId = tripIds[2], Rating = 4, Comment = "Rahat bir yolculuktu, tavsiye ederim.", IsActive = true, CreatedDate = DateTime.Now },
                    new Review { TripId = tripIds[3], Rating = 1, Comment = "Sefer 1 saat rötarlı kalktı.", IsActive = true, CreatedDate = DateTime.Now },
                    new Review { TripId = tripIds[4], Rating = 5, Comment = "Muavin çok ilgiliydi, teşekkürler.", IsActive = true, CreatedDate = DateTime.Now },
                    new Review { TripId = tripIds[5], Rating = 4, Comment = "İnternet bağlantısı koptu ama onun dışında iyiydi.", IsActive = true, CreatedDate = DateTime.Now }
                };
                _context.Set<Review>().AddRange(reviews);
                _context.SaveChanges();
            }

            return Ok("Tüm tablolara en az 6'şar adet mantıklı veri başarıyla eklendi.");
        }
    }
}
