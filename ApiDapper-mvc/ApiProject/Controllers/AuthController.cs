using System.Threading.Tasks;
using ApiProject.Models;
using Microsoft.AspNetCore.Mvc;
using Dapper;

namespace ApiProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDto model)
        {
            // Basit hashleme örneği. BCrypt vs kullanılabilir.
            string hash = BCrypt.Net.BCrypt.HashPassword(model.Password);

            var param = new DynamicParameters();
            param.Add("FullName", model.FullName);
            param.Add("Email", model.Email);
            param.Add("PasswordHash", hash);
            
            try 
            {
                Context.ExecuteReturn("dbo.UserRegister", param);
                return Ok();
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto model)
        {
            var param = new DynamicParameters();
            param.Add("Email", model.Email);
            
            var user = Context.Getir<User>("dbo.UserLogin", param);
            
            if (user == null)
            {
                return Unauthorized("Kullanıcı bulunamadı.");
            }

            bool isValid = BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash);
            
            if (!isValid)
            {
                return Unauthorized("Şifre hatalı.");
            }

            user.PasswordHash = null; // Hassas veriyi temizle
            return Ok(user);
        }
    }
}
