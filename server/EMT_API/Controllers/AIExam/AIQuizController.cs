using EMT_API.Data;
using EMT_API.DTOs.AITest;
using EMT_API.ExternalServices;
using EMT_API.Services;
using EMT_API.Services.AIExam;
using EMT_API.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;

namespace EMT_API.Controllers.AI
{
    [ApiController]
    [Route("api/teacher/ai-quiz")]
    [Authorize(Roles = "TEACHER,ADMIN")]
    public class AIQuizController : ControllerBase
    {
        private readonly IAIQuizService _ai;

        public AIQuizController(AIQuizService ai)
        {
            _ai = ai;
        }

        private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpPost("generate")]
        public async Task<ActionResult<AIQuizResponse>> GenerateQuiz([FromBody] AIQuizRequest req)
        {
            try
            {
                var result = await _ai.GenerateQuizAsync(req);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
