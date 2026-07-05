using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiProject.Data;
using ApiProject.Models;

namespace ApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriversController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DriversController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("GetDriver")]
        public async Task<IEnumerable<Driver>> GetDriver()
        {
            return await _context.Drivers.ToListAsync();
        }

        [HttpPost]
        [Route("PostDriver")]
        public async Task<IActionResult> PostDriver(Driver driver)
        {
            _context.Drivers.Add(driver);
            await _context.SaveChangesAsync();
            return Ok(driver);
        }

        [HttpPut]
        [Route("PutDriver")]
        public async Task<IActionResult> PutDriver(Driver driver)
        {
            _context.Drivers.Update(driver);
            await _context.SaveChangesAsync();
            return Ok(driver);
        }

        [HttpDelete]
        [Route("DeleteDriver/{id}")]
        public async Task<IActionResult> DeleteDriver(int id)
        {
            var driver = await _context.Drivers.FindAsync(id);
            if (driver == null)
            {
                return NotFound();
            }

            _context.Drivers.Remove(driver);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
