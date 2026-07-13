using Microsoft.AspNetCore.Mvc;
using ObiletApp.Core.Entities;
using ObiletApp.Core.Interfaces;
using System.Threading.Tasks;

namespace ObiletApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly IGenericRepository<Location> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public LocationController(IGenericRepository<Location> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Location entity)
        {
            await _repository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Location entity)
        {
            var existingEntity = await _repository.GetByIdAsync(id);
            if (existingEntity == null) return NotFound();

            _repository.Update(entity);
            await _unitOfWork.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingEntity = await _repository.GetByIdAsync(id);
            if (existingEntity == null) return NotFound();

            _repository.Remove(existingEntity);
            await _unitOfWork.SaveChangesAsync();
            return Ok();
        }
    }
}