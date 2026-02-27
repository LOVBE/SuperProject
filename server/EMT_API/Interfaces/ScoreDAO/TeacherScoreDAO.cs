using EMT_API.Data;
using EMT_API.Models;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EMT_API.DAOs.ScoreDAO
{
    public class TeacherScoreDAO : ITeacherScoreDAO
    {
        private readonly EMTDbContext _db;

        public TeacherScoreDAO(EMTDbContext db)
        {
            _db = db;
        }

        public async Task<AnswerTeacherReview> CreateTeacherReviewAsync(AnswerTeacherReview review)
        {
            _db.AnswerTeacherReviews.Add(review);
            await _db.SaveChangesAsync();
            return review;
        }

        public async Task<List<AnswerTeacherReview>> GetAllList (long id)
        {
            return await _db.AnswerTeacherReviews
           .Where(r => r.AIReviewID == id)
           .OrderByDescending(r => r.CreatedAt).ToListAsync();
        }
    
    }
}
