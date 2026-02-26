using EMT_API.DAOs;
using EMT_API.Services.FeedbackService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EMT_API.Controllers.TeacherSide
{
    [ApiController]
    [Route("api/teacher/feedbacks")]
    [Authorize(Roles = "TEACHER")]
    public class TeacherFeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedback;
        public TeacherFeedbackController(IFeedbackService feedback) => _feedback = feedback;

        [HttpGet]
        public async Task<IActionResult> GetMyCourseFeedbacks()
        {
            int teacherId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var feedbacks = await _feedback.GetTeacherFeedbacksAsync(teacherId);
            return Ok(feedbacks);
        }
    }
}
