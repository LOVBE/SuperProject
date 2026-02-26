using EMT_API.Data;
using EMT_API.DTOs.Progress;
using EMT_API.Models;
using Microsoft.EntityFrameworkCore;

namespace EMT_API.DAOs.FlashcardDAO
{
    public class FlashcardProgressDAO : IFlashcardProgressDAO
    {
        private readonly EMTDbContext _db;

        public FlashcardProgressDAO(EMTDbContext db)
        {
            _db = db;
        }

        // ===== Progress =====
        public async Task<FlashcardProgress?> GetProgressAsync(int userId, int itemId)
        {
            return await _db.FlashcardProgresses
                .FirstOrDefaultAsync(p => p.UserID == userId && p.ItemID == itemId);
        }

        public async Task<List<FlashcardProgressDto>> GetProgressBySetAsync(int userId, int setId)
        {
            return await _db.FlashcardProgresses
                .Where(p => p.UserID == userId && p.Item.SetID == setId)
                .Select(p => new FlashcardProgressDto
                {
                    ItemId = p.ItemID,
                    FrontText = p.Item.FrontText,
                    IsMastered = p.IsMastered,
                    ReviewCount = p.ReviewCount,
                    NextReviewAt = p.NextReviewAt
                })
                .ToListAsync();
        }


        public async Task<List<FlashcardProgress>> GetDueForReviewAsync(int userId)
        {
            var now = DateTime.UtcNow;
            return await _db.FlashcardProgresses
                .Where(p => p.UserID == userId &&
                            p.NextReviewAt != null &&
                            p.NextReviewAt <= now)
                .ToListAsync();
        }

        public async Task UpsertProgressAsync(FlashcardProgress progress)
        {
            var existing = await _db.FlashcardProgresses
                .FindAsync(progress.UserID, progress.ItemID);

            if (existing == null)
            {
                _db.FlashcardProgresses.Add(progress);
            }
            else
            {
                _db.Entry(existing).CurrentValues.SetValues(progress);
            }

            await _db.SaveChangesAsync();
        }

        public async Task MarkMasteredAsync(int userId, int itemId)
        {
            var progress = await _db.FlashcardProgresses
                .FindAsync(userId, itemId);

            if (progress == null) return;

            progress.IsMastered = true;
            progress.NextReviewAt = null;
            await _db.SaveChangesAsync();
        }

        // ===== History =====
        public async Task LogHistoryAsync(int userId, int itemId, byte actionType)
        {
            _db.FlashcardHistories.Add(new FlashcardHistory
            {
                UserID = userId,
                ItemID = itemId,
                ActionType = actionType,
                CreatedAt = DateTime.UtcNow
            });

            await _db.SaveChangesAsync();
        }

        public async Task<List<FlashcardHistory>> GetHistoryAsync(int userId, int? setId = null)
        {
            var query = _db.FlashcardHistories
                .Include(h => h.Item)
                .Where(h => h.UserID == userId);

            if (setId.HasValue)
                query = query.Where(h => h.Item.SetID == setId);

            return await query
                .OrderByDescending(h => h.CreatedAt)
                .ToListAsync();
        }
    }
}
