using EMT_API.DAOs.AIDAO;
using EMT_API.DAOs.ScoreDAO;
using EMT_API.DTOs.Score;
using EMT_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace EMT_API.Controllers.TeacherSide
{
    [ApiController]
    [Route("api/teacher/review")]
    [Authorize(Roles = "TEACHER")]
    public class TeacherReviewController : ControllerBase
    {
        private readonly ITeacherScoreDAO _tdao;
        private readonly IAIReviewDAO _aiReviewDao;

        public TeacherReviewController(ITeacherScoreDAO tdao, IAIReviewDAO aiReviewDao)
        {
            _tdao = tdao;
            _aiReviewDao = aiReviewDao;
        }

        private int GetTeacherId()
        => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);


        [HttpPost("updateScore")]
        public async Task<IActionResult> Review([FromBody] CreateTeacherReviewRequest req)
        {
            var aiReview = await _aiReviewDao.GetByIdAsync(req.AIReviewId);
            if (aiReview == null)
                return BadRequest();

            var review = await _tdao.CreateTeacherReviewAsync(new AnswerTeacherReview
            {
                AIReviewID = req.AIReviewId,
                TeacherID = GetTeacherId(),
                ScoreOverall = req.ScoreOverall,
                ScoreTask = req.ScoreTask,
                ScoreLexial = req.ScoreLexical,
                ScoreGrammar = req.ScoreGrammar,
                ScorePronunciation = req.ScorePronunciation,
                ScoreFluency = req.ScoreFluency,
                ScoreCoherence = req.ScoreCoherence,
                Feedback = req.Feedback,
                CreatedAt = DateTime.UtcNow
            });

            return Ok(new
            {
                review.TeacherReviewID,
                review.ScoreOverall,
                review.ScoreTask,
                review.ScoreLexial,
                review.ScoreGrammar,
                review.ScorePronunciation,
                review.ScoreFluency,
                review.ScoreCoherence,
                review.Feedback,
                review.CreatedAt
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetListPending()
        {
            var pending = await _aiReviewDao.GetPendingForTeacherAsync();
            var result = pending.Select(x => new
            {
                x.AIReviewID,
                x.ScoreOverall,
                x.ScoreTask,
                x.ScoreLexical,
                x.ScoreGrammar,
                x.ScorePronunciation,
                x.ScoreFluency,
                x.ScoreCoherence,
                x.Feedback,
                x.CreatedAt
            }); 

            return Ok(result);
        }
    } 
}
