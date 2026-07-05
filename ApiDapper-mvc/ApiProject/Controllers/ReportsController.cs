using System.Linq;
using ApiProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        [HttpGet("roomtyperevenue")]
        public IActionResult GetRoomTypeRevenue()
        {
            var data = Context.Listeleme<RoomTypeRevenueDto>("dbo.Rpt_RoomTypeRevenue").ToList();
            return Ok(data);
        }

        [HttpGet("gueststays")]
        public IActionResult GetGuestStays()
        {
            var data = Context.Listeleme<GuestStayDto>("dbo.Rpt_GuestStays").ToList();
            return Ok(data);
        }

        [HttpGet("roomavailability")]
        public IActionResult GetRoomAvailability()
        {
            var data = Context.Listeleme<RoomAvailabilityDto>("dbo.Rpt_RoomAvailability").ToList();
            return Ok(data);
        }

        [HttpGet("upcomingreservations")]
        public IActionResult GetUpcomingReservations()
        {
            var data = Context.Listeleme<UpcomingReservationDto>("dbo.Rpt_UpcomingReservations").ToList();
            return Ok(data);
        }

        [HttpGet("monthlyrevenue")]
        public IActionResult GetMonthlyRevenue()
        {
            var data = Context.Listeleme<MonthlyRevenueDto>("dbo.Rpt_MonthlyRevenue").ToList();
            return Ok(data);
        }
    }
}
