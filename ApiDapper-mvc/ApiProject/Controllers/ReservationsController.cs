using System.Threading.Tasks;
using ApiProject.Models;
using Microsoft.AspNetCore.Mvc;
using Dapper;

namespace ApiProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            var data = Context.Listeleme<Reservation>("dbo.ReservationViewAll");
            return Ok(data);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var param = new DynamicParameters();
            param.Add("Id", id);
            var data = Context.Getir<Reservation>("dbo.ReservationViewByNo", param);
            if (data == null) return NotFound();
            return Ok(data);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Reservation entity)
        {
            var param = new DynamicParameters();
            param.Add("Id", 0);
            param.Add("GuestId", entity.GuestId);
            param.Add("RoomId", entity.RoomId);
            param.Add("CheckInDate", entity.CheckInDate);
            param.Add("CheckOutDate", entity.CheckOutDate);
            param.Add("TotalPrice", entity.TotalPrice);
            Context.ExecuteReturn("dbo.ReservationEY", param);
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Reservation entity)
        {
            var param = new DynamicParameters();
            param.Add("Id", id);
            param.Add("GuestId", entity.GuestId);
            param.Add("RoomId", entity.RoomId);
            param.Add("CheckInDate", entity.CheckInDate);
            param.Add("CheckOutDate", entity.CheckOutDate);
            param.Add("TotalPrice", entity.TotalPrice);
            Context.ExecuteReturn("dbo.ReservationEY", param);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var param = new DynamicParameters();
            param.Add("Id", id);
            Context.ExecuteReturn("dbo.ReservationSil", param);
            return Ok();
        }
    }
}
