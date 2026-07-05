using System.Threading.Tasks;
using ApiProject.Models;
using Microsoft.AspNetCore.Mvc;
using Dapper;

namespace ApiProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomTypesController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            var data = Context.Listeleme<RoomType>("dbo.RoomTypeViewAll");
            return Ok(data);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var param = new DynamicParameters();
            param.Add("Id", id);
            var data = Context.Getir<RoomType>("dbo.RoomTypeViewByNo", param);
            if (data == null) return NotFound();
            return Ok(data);
        }

        [HttpPost]
        public IActionResult Post([FromBody] RoomType entity)
        {
            var param = new DynamicParameters();
            param.Add("Id", 0);
            param.Add("Name", entity.Name);
            param.Add("Description", entity.Description);
            param.Add("BasePrice", entity.BasePrice);
            Context.ExecuteReturn("dbo.RoomTypeEY", param);
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] RoomType entity)
        {
            var param = new DynamicParameters();
            param.Add("Id", id);
            param.Add("Name", entity.Name);
            param.Add("Description", entity.Description);
            param.Add("BasePrice", entity.BasePrice);
            Context.ExecuteReturn("dbo.RoomTypeEY", param);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var param = new DynamicParameters();
            param.Add("Id", id);
            Context.ExecuteReturn("dbo.RoomTypeSil", param);
            return Ok();
        }
    }
}
