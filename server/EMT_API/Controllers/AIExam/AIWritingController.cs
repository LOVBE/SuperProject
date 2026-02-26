//using EMT_API.Data;
//using EMT_API.DTOs.AIWriting;
//using EMT_API.Models;
//using EMT_API.Services;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System.Security.Claims;
//using EMT_API.Utils;
//using EMT_API.DAOs.MembershipDAO;
//namespace EMT_API.Controllers.AI
//{
//    [ApiController]
//    [Route("api/user/ai-writing")]
//    [Authorize(Roles = "STUDENT")]
//    public class AIWritingController : ControllerBase
//    {
//        private readonly IMembershipDAO _db;
//        private readonly AIWritingService _ai;

//        public AIWritingController(IMembershipDAO db, AIWritingService ai)
//        {
//            _db = db;
//            _ai = ai;
//        }

//        private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);


//        [HttpPost("generate")]
//        public async Task<ActionResult<AIWritingPromptResponse>> GeneratePrompt()
//        {
//            bool hasMembership = await _db.HasActiveMembershipAsync(GetUserId());
//            if (!hasMembership)
//                return StatusCode(403, new { message = "Membership required or expired." });
//            var (title, content) = await _ai.GenerateWritingPromptAsync();
//            return Ok(new AIWritingPromptResponse
//            {
//                Title = title,
//                Content = content
//            });
//        }

//        [HttpPost("submit")]
//        public async Task<ActionResult<AIWritingFeedbackResponse>> Submit([FromBody] AIWritingSubmitRequest req)
//        {
//            bool hasMembership = await _db.HasActiveMembershipAsync(GetUserId());

//            if (!hasMembership)
//                return StatusCode(403, new { message = "Membership required or expired." });
//            if (req == null || string.IsNullOrWhiteSpace(req.AnswerText))
//                return BadRequest("AnswerText cannot be empty.");
//            if (string.IsNullOrWhiteSpace(req.PromptContent))
//                return BadRequest("Missing AI-generated generated question.");

//            // Gọi AI chấm điểm
//            var (overall, task, coherence, lexical, grammar, feedback) =
//                await _ai.GradeWritingAsync(req.AnswerText);

//            return Ok(new AIWritingFeedbackResponse
//            {
//                Score = overall,
//                TaskResponse = task,
//                Coherence = coherence,
//                LexicalResource = lexical,
//                Grammar = grammar,
//                Feedback = feedback
//            });
//        }


//    }
//}


using EMT_API.DTOs.AIWriting;
using EMT_API.ExternalServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using EMT_API.DAOs.MembershipDAO;

namespace EMT_API.Controllers.AI
{
    [ApiController]
    [Route("api/user/ai-writing")]
    [Authorize(Roles = "STUDENT")]
    public class AIWritingController : ControllerBase
    {
        private readonly IMembershipDAO _membershipDao;
        private readonly AIWritingService _ai;

        public AIWritingController(
            IMembershipDAO membershipDao,
            AIWritingService ai)
        {
            _membershipDao = membershipDao;
            _ai = ai;
        }

        private int GetUserId()
            => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        // =========================
        // 1️⃣ Generate Writing Prompt
        // =========================
        [HttpPost("generate")]
        public async Task<ActionResult<AIWritingPromptResponse>> GeneratePrompt()
        {
            if (!await _membershipDao.HasActiveMembershipAsync(GetUserId()))
                return StatusCode(403, new { message = "Membership required or expired." });

            var (title, content) = await _ai.GenerateWritingPromptAsync();

            // ⚠️ PromptId sẽ lấy từ DB (AIPrompt)
            return Ok(new AIWritingPromptResponse
            {
                PromptId = 0, // TODO: gán từ DB
                Title = title,
                Content = content
            });
        }

        // =========================
        // 2️⃣ Submit Writing (AI chấm)
        // =========================
        [HttpPost("submit")]
        public async Task<ActionResult<AIWritingFeedbackResponse>> Submit(
            [FromBody] AIWritingSubmitRequest req)
        {
            if (!await _membershipDao.HasActiveMembershipAsync(GetUserId()))
                return StatusCode(403, new { message = "Membership required or expired." });

            if (req == null || string.IsNullOrWhiteSpace(req.AnswerText))
                return BadRequest(new { message = "AnswerText cannot be empty." });

            // ⚠️ Load prompt từ DB bằng req.PromptId
            // var prompt = await _aiPromptDao.GetByIdAsync(req.PromptId);

            var (overall, task, coherence, lexical, grammar, feedback) =
                await _ai.GradeWritingAsync(req.AnswerText);

            // ⚠️ LƯU:
            // - AISubmission
            // - AnswerAIReview nếu req.SendToTeacher == true
            // → để trong AIDAO

            return Ok(new AIWritingFeedbackResponse
            {
                Score = overall,
                TaskResponse = task,
                Coherence = coherence,
                LexicalResource = lexical,
                Grammar = grammar,
                Feedback = feedback
            });
        }
    }
}
