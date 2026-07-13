using Microsoft.AspNetCore.Mvc;
using ObiletApp.Core.Interfaces;
using System.Threading.Tasks;

namespace ObiletApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AiController : ControllerBase
    {
        private readonly IAiAssistantService _aiService;

        public AiController(IAiAssistantService aiService)
        {
            _aiService = aiService;
        }

        [HttpGet("ask")]
        public async Task<IActionResult> Ask([FromQuery] string question)
        {
            if (string.IsNullOrWhiteSpace(question))
                return BadRequest("Lütfen bir soru sorun.");

            var answer = await _aiService.AskQuestionAsync(question);
            return Ok(new { Answer = answer });
        }
    }
}
