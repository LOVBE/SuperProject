using EMT_API.DAOs.FlashcardDAO;
using EMT_API.DTOs.Flashcard;
using EMT_API.DTOs.Progress;
using EMT_API.Models;
using Google.Apis.Drive.v3.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/flashcard/progress")]
[Authorize]
public class FlashcardProgressController : ControllerBase
{
    private readonly IFlashcardProgressDAO _progressDao;

    public FlashcardProgressController(IFlashcardProgressDAO progressDao)
    {
        _progressDao = progressDao;
    }

    private int? GetUserId()
    {
        var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                    ?? User.FindFirst("sub")?.Value;

        return string.IsNullOrEmpty(idClaim) ? null : int.Parse(idClaim);
    }

    // =========================
    // 1️⃣ Lấy tiến độ học theo set
    // =========================
    [HttpGet("set/{setId:int}")]
    public async Task<IActionResult> GetProgressBySet(int setId)
    {
        var userId = GetUserId();
        if (userId == null)
            return Unauthorized(new { message = "Login required." });

        try
        {
            var progress = await _progressDao.GetProgressBySetAsync(userId.Value, setId);

            if (progress == null || !progress.Any())
                return NotFound(new { message = "No progress found for this set." });

            return Ok(progress);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Failed to get flashcard progress.",
                detail = ex.Message
            });
        }
    }

    // =========================
    // 4️⃣ Ghi lịch sử học
    // =========================
    [HttpPost("learn")]
    public async Task<IActionResult> Learn([FromBody] LearnFlashcardRequest request)
    {
        var userId = GetUserId();
        if (userId == null)
            return Unauthorized(new { message = "Login required." });

        if (request == null)
            return BadRequest(new { message = "Invalid request body." });

        try
        {
            // update progress
            await _progressDao.UpsertProgressAsync(new FlashcardProgress
            {
                UserID = userId.Value,
                ItemID = request.ItemId,
                IsMastered = request.ActionType == FlashcardActionType.Mastered,
                ReviewCount = 1,
                NextReviewAt = DateTime.UtcNow.AddDays(1)
            });

            // log history
            await _progressDao.LogHistoryAsync(
                userId.Value,
                request.ItemId,
                (byte)request.ActionType
            );

            return Ok(new { message = "Learn recorded." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Failed to learn flashcard.",
                detail = ex.Message
            });
        }
    }

    // =========================
    // 5️⃣ Lấy lịch sử học
    // =========================
    [HttpGet("history")]
    public async Task<IActionResult> GetHistory([FromQuery] int? setId)
    {
        var userId = GetUserId();
        if (userId == null)
            return Unauthorized(new { message = "Login required." });

        try
        {
            var history = await _progressDao.GetHistoryAsync(userId.Value, setId);

            if (history == null || !history.Any())
                return NotFound(new { message = "No study history found." });

            return Ok(history);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Failed to get flashcard history.",
                detail = ex.Message
            });
        }
    }
}
