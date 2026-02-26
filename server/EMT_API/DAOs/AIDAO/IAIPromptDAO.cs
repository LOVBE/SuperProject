using EMT_API.Models;

namespace EMT_API.DAOs.AIDAO
{
    public interface IAIPromptDAO
    {
        Task<AIPrompt> CreateAsync(AIPrompt prompt);
        Task<AIPrompt?> GetByIdAsync(int promptId);
    }
}
