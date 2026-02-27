using EMT_API.DAOs.AIDAO;
using EMT_API.Data;
using EMT_API.Models;
using Microsoft.EntityFrameworkCore;

namespace EMT_API.DAOs.AIDAO
{
    public class AIPromptDAO : IAIPromptDAO
    {
        private readonly EMTDbContext _db;

        public AIPromptDAO(EMTDbContext db)
        {
            _db = db;
        }

        public async Task<AIPrompt> CreateAsync(AIPrompt prompt)
        {
            _db.AIPrompts.Add(prompt);
            await _db.SaveChangesAsync();
            return prompt;
        }

        public async Task<AIPrompt?> GetByIdAsync(int promptId)
        {
            return await _db.AIPrompts.FindAsync(promptId);
        }
    }
}
