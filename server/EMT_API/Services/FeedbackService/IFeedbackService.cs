using EMT_API.DTOs.Feedback;

namespace EMT_API.Services.FeedbackService
{
    public interface IFeedbackService
    {
        Task<IEnumerable<FeedbackViewDto>> GetTeacherFeedbacksAsync(int teacherId);
    }
}
