using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiProject.Data;
using ApiProject.Models;

namespace ApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeposController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DeposController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("GetDepo")]
        public async Task<IEnumerable<Depo>> GetDepo()
        {
            return await _context.Depos.ToListAsync();
        }

        [HttpPost]
        [Route("PostDepo")]
        public async Task<IActionResult> PostDepo(Depo depo)
        {
            _context.Depos.Add(depo);
            await _context.SaveChangesAsync();
            return Ok(depo);
        }

        [HttpPut]
        [Route("PutDepo")]
        public async Task<IActionResult> PutDepo(Depo depo)
        {
            _context.Depos.Update(depo);
            await _context.SaveChangesAsync();
            return Ok(depo);
        }

        [HttpDelete]
        [Route("DeleteDepo/{id}")]
        public async Task<IActionResult> DeleteDepo(int id)
        {
            var depo = await _context.Depos.FindAsync(id);
            if (depo == null)
            {
                return NotFound();
            }

            _context.Depos.Remove(depo);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
