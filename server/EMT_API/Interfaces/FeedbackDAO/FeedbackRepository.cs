using EMT_API.Data;
using EMT_API.DTOs.Feedback;
using EMT_API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMT_API.DAOs
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly EMTDbContext _db;
        public FeedbackRepository(EMTDbContext db) => _db = db;

        // ----- USER -----
        public async Task<bool> HasFeedbackAsync(int courseId, int userId)
        {
            return await _db.Feedbacks.AnyAsync(f => f.CourseID == courseId && f.UserID == userId);
        }

        public async Task<Feedback> CreateFeedbackAsync(Feedback feedback)
        {
            _db.Feedbacks.Add(feedback);
            await _db.SaveChangesAsync();
            return feedback;
        }

        // ----- PUBLIC -----
        public async Task<(double average, int total)> GetCourseRatingAsync(int courseId)
        {
            var query = _db.Feedbacks.Where(f => f.CourseID == courseId && f.IsVisible);
            var count = await query.CountAsync();
            if (count == 0) return (0, 0);
            var avg = await query.AverageAsync(f => (double)f.Rating);
            return (avg, count);
        }

        public async Task<List<FeedbackViewDto>> GetCourseFeedbacksAsync(int courseId)
        {
            return await _db.Feedbacks
                .Include(f => f.User)
                .Where(f => f.CourseID == courseId && f.IsVisible)
                .OrderByDescending(f => f.CreatedAt)
                .Select(f => new FeedbackViewDto
                {
                    FeedbackId = f.FeedbackID,
                    UserId = f.UserID,
                    Username = f.User.Username,
                    CourseId = f.CourseID,
                    CourseName = f.Course.CourseName,
                    Rating = f.Rating,
                    Comment = f.Comment,
                    CreatedAt = f.CreatedAt,
                    IsVisible = f.IsVisible
                })
                .ToListAsync();
        }

        // ----- TEACHER -----
        public async Task<List<Feedback>> GetTeacherFeedbacksAsync(int teacherId)
        {
            return await _db.Feedbacks
                .Include(f => f.Course)
                .Include(f => f.User)
                .Where(f => f.Course.TeacherID == teacherId && f.IsVisible)
                .OrderByDescending(f => f.CreatedAt)
                .ToListAsync();
        }

        // ----- ADMIN -----
        public async Task<List<FeedbackViewDto>> GetAllFeedbacksAsync()
        {
            return await _db.Feedbacks
                .Include(f => f.Course)
                .Include(f => f.User)
                .OrderByDescending(f => f.CreatedAt)
                .Select(f => new FeedbackViewDto
                {
                    FeedbackId = f.FeedbackID,
                    UserId = f.UserID,
                    Username = f.User.Username,
                    CourseId = f.CourseID,
                    CourseName = f.Course.CourseName,
                    Rating = f.Rating,
                    Comment = f.Comment,
                    CreatedAt = f.CreatedAt,
                    IsVisible = f.IsVisible
                })
                .ToListAsync();
        }

        public async Task<bool> ToggleVisibilityAsync(int feedbackId)
        {
            var feedback = await _db.Feedbacks.FindAsync(feedbackId);
            if (feedback == null) return false;
            feedback.IsVisible = !feedback.IsVisible;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteFeedbackAsync(int feedbackId)
        {
            var fb = await _db.Feedbacks.FindAsync(feedbackId);
            if (fb == null) return false;
            _db.Feedbacks.Remove(fb);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
