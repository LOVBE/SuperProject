using EMT_API.Data;
using EMT_API.Models;
using Microsoft.EntityFrameworkCore;

namespace EMT_API.DAOs.AIDAO
{
    public class AIReviewDAO : IAIReviewDAO
    {
        private readonly EMTDbContext _db;

        public AIReviewDAO(EMTDbContext db)
        {
            _db = db;
        }

        public async Task<AnswerAIReview> CreateAsync(AnswerAIReview review)
        {
            _db.AnswerAIReviews.Add(review);
            await _db.SaveChangesAsync();
            return review;
        }

        public async Task<AnswerAIReview?> GetBySubmissionAsync(long submissionId)
        {
            return await _db.AnswerAIReviews
                .Include(r => r.Submission)
                .FirstOrDefaultAsync(r => r.SubmissionID == submissionId);
        }

        public async Task<List<AnswerAIReview>> GetPendingForTeacherAsync()
        {
            return await _db.AnswerAIReviews
                .Include(r => r.Submission)
                .Where(r => r.IsSentToTeacher)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<AnswerAIReview> GetByIdAsync(long id)
        {
            return await _db.AnswerAIReviews.FirstOrDefaultAsync(a => a.AIReviewID == id);
        }
    }
}
