using System.Threading.Tasks;
using ApiProject.Models;
using Microsoft.AspNetCore.Mvc;
using Dapper;

namespace ApiProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            var data = Context.Listeleme<Room>("dbo.RoomViewAll");
            return Ok(data);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var param = new DynamicParameters();
            param.Add("Id", id);
            var data = Context.Getir<Room>("dbo.RoomViewByNo", param);
            if (data == null) return NotFound();
            return Ok(data);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Room entity)
        {
            var param = new DynamicParameters();
            param.Add("Id", 0);
            param.Add("RoomNumber", entity.RoomNumber);
            param.Add("RoomTypeId", entity.RoomTypeId);
            param.Add("IsAvailable", entity.IsAvailable);
            Context.ExecuteReturn("dbo.RoomEY", param);
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Room entity)
        {
            var param = new DynamicParameters();
            param.Add("Id", id);
            param.Add("RoomNumber", entity.RoomNumber);
            param.Add("RoomTypeId", entity.RoomTypeId);
            param.Add("IsAvailable", entity.IsAvailable);
            Context.ExecuteReturn("dbo.RoomEY", param);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var param = new DynamicParameters();
            param.Add("Id", id);
            Context.ExecuteReturn("dbo.RoomSil", param);
            return Ok();
        }
    }
}
