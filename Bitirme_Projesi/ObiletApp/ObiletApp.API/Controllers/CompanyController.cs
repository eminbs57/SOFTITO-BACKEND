using Microsoft.AspNetCore.Mvc;
using ObiletApp.Core.Entities;
using ObiletApp.Core.Interfaces;
using System.Threading.Tasks;

namespace ObiletApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly IGenericRepository<Company> _companyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CompanyController(IGenericRepository<Company> companyRepository, IUnitOfWork unitOfWork)
        {
            _companyRepository = companyRepository;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Company company)
        {
            await _companyRepository.AddAsync(company);
            await _unitOfWork.SaveChangesAsync();
            return Ok(company);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Company company)
        {
            if (id != company.Id) return BadRequest("ID mismatch");

            _companyRepository.Update(company);
            await _unitOfWork.SaveChangesAsync();
            return Ok(company);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var company = await _companyRepository.GetByIdAsync(id);
            if (company == null) return NotFound();

            _companyRepository.Remove(company);
            await _unitOfWork.SaveChangesAsync();
            return Ok();
        }
    }
}
