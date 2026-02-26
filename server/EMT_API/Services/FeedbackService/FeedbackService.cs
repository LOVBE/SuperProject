using EMT_API.DAOs;
using EMT_API.DTOs.Feedback;
using EMT_API.Models;

namespace EMT_API.Services.FeedbackService
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedback;
        
        public FeedbackService(IFeedbackRepository feedback)
        {
            _feedback = feedback;
        }

        public async Task<IEnumerable<FeedbackViewDto>> GetTeacherFeedbacksAsync(int teacherId)
        {
            var feedbacks = await _feedback.GetTeacherFeedbacksAsync(teacherId);

            return feedbacks.Select(f => new FeedbackViewDto
            {
                FeedbackId = f.FeedbackID,
                UserId = f.UserID,
                Username = f.User.Username,
                CourseId = f.CourseID,
                CourseName = f.Course.CourseName,
                Rating = f.Rating,
                Comment = f.Comment,
                CreatedAt = f.CreatedAt,
                IsVisible = f.IsVisible
            });
        }
    }
}
