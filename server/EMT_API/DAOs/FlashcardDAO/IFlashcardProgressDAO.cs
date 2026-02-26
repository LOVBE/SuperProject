using EMT_API.DTOs.Progress;
using EMT_API.Models;

namespace EMT_API.DAOs.FlashcardDAO
{
    public interface IFlashcardProgressDAO
    {
        // ===== Progress =====
        Task<FlashcardProgress?> GetProgressAsync(int userId, int itemId);
        Task<List<FlashcardProgressDto>> GetProgressBySetAsync(int userId, int setId);
        Task<List<FlashcardProgress>> GetDueForReviewAsync(int userId);

        Task UpsertProgressAsync(FlashcardProgress progress);
        Task MarkMasteredAsync(int userId, int itemId);

        // ===== History =====
        Task LogHistoryAsync(int userId, int itemId, byte actionType);
        Task<List<FlashcardHistory>> GetHistoryAsync(int userId, int? setId = null);
    }
}
