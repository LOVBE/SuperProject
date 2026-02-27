using EMT_API.Models;

namespace EMT_API.DAOs.AIDAO
{
    public interface IAISubmissionDAO
    {
        Task<AISubmission> CreateAsync(AISubmission submission);
        Task<AISubmission?> GetByIdAsync(long submissionId);
        Task<List<AISubmission>> GetByUserAsync(int userId);
    }
}
