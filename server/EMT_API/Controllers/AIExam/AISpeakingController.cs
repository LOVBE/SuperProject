using EMT_API.DAOs.AIDAO;
using EMT_API.DAOs.MembershipDAO;
using EMT_API.DTOs.AITest;
using EMT_API.ExternalServices;
using EMT_API.Models;
using EMT_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EMT_API.Controllers.AI
{
    [ApiController]
    [Route("api/user/ai-speaking")]
    [Authorize(Roles = "STUDENT")]
    public class AISpeakingController : ControllerBase
    {
        private readonly AISpeakingService _ai;
        private readonly IMembershipDAO _membershipDao;
        private readonly IAIPromptDAO _promptDao;
        private readonly IAISubmissionDAO _submissionDao;
        private readonly IAIReviewDAO _reviewDao;
        private readonly ILogger<AISpeakingController> _logger;

        public AISpeakingController(
            AISpeakingService ai,
            IMembershipDAO membershipDao,
            IAIPromptDAO promptDao,
            IAISubmissionDAO submissionDao,
            IAIReviewDAO reviewDao,
            ILogger<AISpeakingController> logger)
        {
            _ai = ai;
            _membershipDao = membershipDao;
            _promptDao = promptDao;
            _submissionDao = submissionDao;
            _reviewDao = reviewDao;
            _logger = logger;
        }


        // =========================
        // Helper
        // =========================
        private int GetUserId()
            => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        private async Task<bool> HasMembershipAsync()
            => await _membershipDao.HasActiveMembershipAsync(GetUserId());

        // =========================
        // 1️⃣ Generate Speaking Prompt
        // =========================
        [HttpPost("generate")]
        public async Task<IActionResult> GeneratePrompt()
        {
            if (!await HasMembershipAsync())
                return StatusCode(403, new { message = "Membership required or expired." });

            var (title, content) = await _ai.GenerateSpeakingPromptAsync();

            var prompt = await _promptDao.CreateAsync(new AIPrompt
            {
                SkillType = "SPEAKING",
                Title = title,
                Content = content,
                CreatedAt = DateTime.UtcNow
            });

            return Ok(new
            {
                PromptId = prompt.PromptID,
                Title = prompt.Title,
                Content = prompt.Content
            });
        }

        // =========================
        // 2️⃣ Submit Speaking (AI chấm)
        // =========================
        [HttpPost("submit")]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(25_000_000)]
        public async Task<IActionResult> Submit([FromForm] AISpeakingSubmitAudioRequest req)
        {
            try
            {
                _logger.LogInformation("=== Starting AI Speaking Submit ===");
                _logger.LogInformation("PromptId: {PromptId}, SendToTeacher: {SendToTeacher}, FileSize: {Size}",
                    req.PromptId, req.SendToTeacher, req.File?.Length);

                if (!await _membershipDao.HasActiveMembershipAsync(GetUserId()))
                {
                    _logger.LogWarning("User {UserId} has no active membership", GetUserId());
                    return StatusCode(403, new { message = "Membership required or expired." });
                }

                if (req.File == null || req.File.Length == 0)
                {
                    _logger.LogWarning("Invalid file submitted");
                    return BadRequest(new { message = "Invalid audio file." });
                }

                // 1️⃣ Load prompt từ DB
                _logger.LogInformation("Loading prompt {PromptId}", req.PromptId);
                var prompt = await _promptDao.GetByIdAsync(req.PromptId);
                if (prompt == null)
                {
                    _logger.LogWarning("Prompt {PromptId} not found", req.PromptId);
                    return BadRequest(new { message = "Invalid PromptId." });
                }

                // 2️⃣ Transcribe
                _logger.LogInformation("Starting transcription...");
                var transcript = await _ai.TranscribeAsync(req.File);

                if (string.IsNullOrWhiteSpace(transcript))
                {
                    _logger.LogWarning("Empty transcript returned");
                    return BadRequest(new
                    {
                        message = "Audio could not be transcribed. Please speak clearly and try again."
                    });
                }

                // 3️⃣ AI grade
                _logger.LogInformation("Starting grading with transcript length: {Length}", transcript.Length);
                var (score, flu, lex, gra, pro, fb) =
                    await _ai.GradeSpeakingAsync(transcript, prompt.Content);

                // 4️⃣ Save to DB if needed
                if (req.SendToTeacher)
                {
                    _logger.LogInformation("Saving submission to database...");
                    var submission = await _submissionDao.CreateAsync(new AISubmission
                    {
                        PromptID = prompt.PromptID,
                        UserID = GetUserId(),
                        AnswerText = null,
                        AudioUrl = null,
                        Transcript = transcript,
                        CreatedAt = DateTime.UtcNow
                    });

                    await _reviewDao.CreateAsync(new AnswerAIReview
                    {
                        SubmissionID = submission.SubmissionID,
                        ModelName = "gpt-4o-mini",
                        ScoreOverall = score,
                        ScoreLexical = lex,
                        ScoreGrammar = gra,
                        ScorePronunciation = pro,
                        Feedback = fb,
                        IsSentToTeacher = true,
                        CreatedAt = DateTime.UtcNow
                    });
                    _logger.LogInformation("Submission saved successfully");
                }

                _logger.LogInformation("=== AI Speaking Submit Completed Successfully ===");

                return Ok(new
                {
                    Transcript = transcript,
                    Score = score,
                    Fluency = flu,
                    LexicalResource = lex,
                    Grammar = gra,
                    Pronunciation = pro,
                    Feedback = fb
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AI Speaking submit failed - Exception details: {Message}", ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError(ex.InnerException, "Inner exception: {Message}", ex.InnerException.Message);
                }
                return StatusCode(500, new { message = "Internal error while grading.", detail = ex.Message });
            }
        }
    }
}
