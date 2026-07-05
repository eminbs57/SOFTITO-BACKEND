using System.Threading.Tasks;
using ApiProject.Models;
using Microsoft.AspNetCore.Mvc;
using Dapper;

namespace ApiProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GuestsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            var data = Context.Listeleme<Guest>("dbo.GuestViewAll");
            return Ok(data);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var param = new DynamicParameters();
            param.Add("Id", id);
            var data = Context.Getir<Guest>("dbo.GuestViewByNo", param);
            if (data == null) return NotFound();
            return Ok(data);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Guest entity)
        {
            var param = new DynamicParameters();
            param.Add("Id", 0);
            param.Add("FirstName", entity.FirstName);
            param.Add("LastName", entity.LastName);
            param.Add("IdentityNumber", entity.IdentityNumber);
            param.Add("Phone", entity.Phone);
            Context.ExecuteReturn("dbo.GuestEY", param);
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Guest entity)
        {
            var param = new DynamicParameters();
            param.Add("Id", id);
            param.Add("FirstName", entity.FirstName);
            param.Add("LastName", entity.LastName);
            param.Add("IdentityNumber", entity.IdentityNumber);
            param.Add("Phone", entity.Phone);
            Context.ExecuteReturn("dbo.GuestEY", param);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var param = new DynamicParameters();
            param.Add("Id", id);
            Context.ExecuteReturn("dbo.GuestSil", param);
            return Ok();
        }
    }
}
