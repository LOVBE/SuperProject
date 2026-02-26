using EMT_API.DTOs.AITest;

namespace EMT_API.Services.AIExam
{
    public interface IAIQuizService
    {
        Task<AIQuizResponse> GenerateQuizAsync(AIQuizRequest request);
    }
}
