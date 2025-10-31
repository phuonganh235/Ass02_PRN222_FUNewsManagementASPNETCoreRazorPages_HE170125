using BusinessLogic.Interfaces;
using BusinessLogic.ViewModels;
using DataAccess.Context;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public class CommentService : ICommentService
    {
        private readonly FUNewsDbContext _context;

        public CommentService(FUNewsDbContext context)
        {
            _context = context;
        }

        public async Task<List<CommentVM>> GetAllAsync()
        {
            return await _context.Comments
                .Include(c => c.Account)
                .Include(c => c.NewsArticle)
                .OrderByDescending(c => c.CreatedAt)
                .Select(c => new CommentVM
                {
                    CommentID = c.CommentID,
                    NewsArticleID = c.NewsArticleID,
                    CommentBy = c.Account != null ? c.Account.AccountName : "Unknown",
                    CommentText = c.Content ?? "",
                    CommentDate = c.CreatedAt
                })

                .ToListAsync();
        }

        public async Task<List<CommentVM>> GetByNewsAsync(string newsArticleId)
        {
            return await _context.Comments
                .Include(c => c.Account)
                .Where(c => c.NewsArticleID == newsArticleId)
                .OrderByDescending(c => c.CreatedAt)
                .Select(c => new CommentVM
                {
                    CommentID = c.CommentID,
                    NewsArticleID = c.NewsArticleID,
                    CommentBy = c.Account != null ? c.Account.AccountName : "Unknown",
                    CommentText = c.Content ?? "",
                    CommentDate = c.CreatedAt
                })

                .ToListAsync();
        }

        public async Task<bool> AddAsync(string newsArticleId, string commentText, string commentBy)
        {
            try
            {
                var account = await _context.SystemAccounts
                    .FirstOrDefaultAsync(a => a.AccountName == commentBy);

                var comment = new Comment
                {
                    NewsArticleID = newsArticleId,
                    AccountID = (short)(account?.AccountID ?? 0),
                    Content = commentText,
                    CreatedAt = DateTime.Now
                };

                _context.Comments.Add(comment);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int commentId)
        {
            var c = await _context.Comments.FindAsync(commentId);
            if (c == null) return false;

            _context.Comments.Remove(c);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
