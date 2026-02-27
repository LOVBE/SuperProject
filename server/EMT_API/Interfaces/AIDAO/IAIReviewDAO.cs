using EMT_API.Models;

namespace EMT_API.DAOs.AIDAO
{
    public interface IAIReviewDAO
    {
        Task<AnswerAIReview> CreateAsync(AnswerAIReview review);
        Task<AnswerAIReview?> GetBySubmissionAsync(long submissionId);
        Task<List<AnswerAIReview>> GetPendingForTeacherAsync();
        Task<AnswerAIReview> GetByIdAsync(long id);
    }
}
