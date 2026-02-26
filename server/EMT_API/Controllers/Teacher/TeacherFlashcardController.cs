using EMT_API.DTOs.Flashcard;
using EMT_API.DTOs.FlashCard;
using EMT_API.DAOs.FlashcardDAO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EMT_API.Controllers.TeacherSide
{
    [ApiController]
    [Route("api/teacher/flashcard")]
    [Authorize(Roles = "TEACHER")]
    public class TeacherFlashcardController : ControllerBase
    {
        private readonly IFlashcardDAO _dao;
        private readonly EMT_API.Data.EMTDbContext _db;

        public TeacherFlashcardController(IFlashcardDAO dao, EMT_API.Data.EMTDbContext db)
        {
            _dao = dao;
            _db = db;
        }

        private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        private async Task<bool> EnsureTeacherOwnsCourse(int courseId)
        {
            var uid = GetUserId();
            var course = await _db.Courses.AsNoTracking()
                .FirstOrDefaultAsync(c => c.CourseID == courseId);
            return course != null && course.TeacherID == uid;
        }

        private async Task<bool> EnsureTeacherOwnsSet(int setId)
        {
            var set = await _dao.GetSetDetailAsync(setId);
            if (set == null) return false;
            if (set.CourseID == null) return true;
            return await EnsureTeacherOwnsCourse(set.CourseID.Value);
        }

        [HttpGet("sets/public")]
        public async Task<IActionResult> GetAllPublicSets()
        {
            var sets = await _dao.GetAllPublicSetsAsync();
            var dtos = sets.Select(s => new FlashcardSetDto
            {
                SetID = s.SetID,
                Title = s.Title,
                Description = s.Description
            });
            return Ok(dtos);
        }

        [HttpGet("sets/course/{courseId:int}")]
        public async Task<IActionResult> GetAllSetsByCourse(int courseId)
        {
            if (!await EnsureTeacherOwnsCourse(courseId))
                return Forbid();

            var sets = await _dao.GetSetsByCourseAsync(courseId);
            var dtos = sets.Select(s => new FlashcardSetDto
            {
                SetID = s.SetID,
                CourseID = s.CourseID,
                Title = s.Title,
                Description = s.Description
            });
            return Ok(dtos);
        }

        [HttpGet("set/{setId:int}")]
        public async Task<IActionResult> GetSetDetail(int setId)
        {
            if (!await EnsureTeacherOwnsSet(setId))
                return Forbid();

            var set = await _dao.GetSetDetailAsync(setId);
            if (set == null) return NotFound(new { message = "Flashcard set not found" });

            var dto = new FlashcardSetDetailDto
            {
                SetID = set.SetID,
                CourseID = set.CourseID,
                Title = set.Title,
                Description = set.Description,
                Items = set.FlashcardItems.Select(i => new FlashcardItemDto
                {
                    ItemID = i.ItemID,
                    FrontText = i.FrontText,
                    BackText = i.BackText,
                    Example = i.Example
                }).ToList()
            };

            return Ok(dto);
        }

        [HttpPost("set")]
        public async Task<IActionResult> CreateSet([FromBody] CreateFlashcardSetRequest req)
        {
            if (req.CourseID.HasValue && !await EnsureTeacherOwnsCourse(req.CourseID.Value))
                return Forbid();

            var set = new EMT_API.Models.FlashcardSet
            {
                CourseID = req.CourseID,
                Title = req.Title,
                Description = req.Description,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _dao.CreateSetAsync(set);
            return Ok(new { message = "Flashcard set created successfully", setId = created.SetID });
        }

        [HttpPost("item")]
        public async Task<IActionResult> AddItem([FromBody] CreateFlashcardItemRequest req)
        {
            if (!await EnsureTeacherOwnsSet(req.SetID))
                return Forbid();

            var item = new EMT_API.Models.FlashcardItem
            {
                SetID = req.SetID,
                FrontText = req.FrontText,
                BackText = req.BackText,
                Example = req.Example,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _dao.CreateItemAsync(item);
            return Ok(new { message = "Flashcard item added successfully", itemId = created.ItemID });
        }

        [HttpPut("set/{setId:int}")]
        public async Task<IActionResult> UpdateSet(int setId, [FromBody] UpdateFlashcardSetRequest req)
        {
            if (!await EnsureTeacherOwnsSet(setId))
                return Forbid();

            var set = new EMT_API.Models.FlashcardSet
            {
                SetID = setId,
                Title = req.Title,
                Description = req.Description
            };

            var ok = await _dao.UpdateSetAsync(set);
            return ok ? Ok(new { message = "Updated" }) : NotFound();
        }

        [HttpPut("item/{itemId:int}")]
        public async Task<IActionResult> UpdateItem(int itemId, [FromBody] CreateFlashcardItemRequest req)
        {
            var item = await _db.FlashcardItems.FindAsync(itemId);
            if (item == null) return NotFound();

            if (!await EnsureTeacherOwnsSet(item.SetID))
                return Forbid();

            item.FrontText = req.FrontText;
            item.BackText = req.BackText;
            item.Example = req.Example;

            var ok = await _dao.UpdateItemAsync(item);
            return ok ? Ok(new { message = "Updated" }) : NotFound();
        }

        [HttpDelete("set/{setId:int}")]
        public async Task<IActionResult> DeleteSet(int setId)
        {
            if (!await EnsureTeacherOwnsSet(setId))
                return Forbid();

            var ok = await _dao.DeleteSetAsync(setId);
            return ok ? Ok(new { message = "Deleted" }) : NotFound();
        }

        [HttpDelete("item/{itemId:int}")]
        public async Task<IActionResult> DeleteItem(int itemId)
        {
            var item = await _db.FlashcardItems.FindAsync(itemId);
            if (item == null) return NotFound();

            if (!await EnsureTeacherOwnsSet(item.SetID))
                return Forbid();

            var ok = await _dao.DeleteItemAsync(itemId);
            return ok ? Ok(new { message = "Deleted" }) : NotFound();
        }
    }
}
