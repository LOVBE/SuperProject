using EMT_API.DAOs.AIDAO;
using EMT_API.Data;
using EMT_API.Models;
using Microsoft.EntityFrameworkCore;

namespace EMT_API.DAOs.AIDAO
{
    public class AISubmissionDAO : IAISubmissionDAO
    {
        private readonly EMTDbContext _db;

        public AISubmissionDAO(EMTDbContext db)
        {
            _db = db;
        }

        public async Task<AISubmission> CreateAsync(AISubmission submission)
        {
            _db.AISubmissions.Add(submission);
            await _db.SaveChangesAsync();
            return submission;
        }

        public async Task<AISubmission?> GetByIdAsync(long submissionId)
        {
            return await _db.AISubmissions
                .Include(s => s.Prompt)
                .FirstOrDefaultAsync(s => s.SubmissionID == submissionId);
        }

        public async Task<List<AISubmission>> GetByUserAsync(int userId)
        {
            return await _db.AISubmissions
                .Include(s => s.Prompt)
                .Where(s => s.UserID == userId)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        }
    }
}
