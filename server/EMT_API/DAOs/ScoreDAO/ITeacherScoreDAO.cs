using EMT_API.Models;

namespace EMT_API.DAOs.ScoreDAO
{
    public interface ITeacherScoreDAO
    {
        Task<AnswerTeacherReview> CreateTeacherReviewAsync(AnswerTeacherReview review);
        Task<List<AnswerTeacherReview>> GetAllList(long id);
    }
}
