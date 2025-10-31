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

        // Lấy tất cả comment (cho admin)
        public async Task<List<CommentVM>> GetAllAsync()
        {
            var comments = await _context.Comments
                .Include(c => c.NewsArticle)
                .Include(c => c.Account)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            return comments.Select(c => new CommentVM
            {
                CommentID = c.CommentID,
                NewsArticleID = c.NewsArticleID,
                CommentBy = c.Account?.AccountName ?? "Unknown",
                CommentText = c.Content,
                CommentDate = c.CreatedAt
            }).ToList();
        }

        // Lấy comment theo bài viết
        public async Task<List<CommentVM>> GetByNewsAsync(int newsArticleId)
        {
            var comments = await _context.Comments
                .Include(c => c.Account)
                .Where(c => c.NewsArticleID == newsArticleId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            return comments.Select(c => new CommentVM
            {
                CommentID = c.CommentID,
                NewsArticleID = c.NewsArticleID,
                CommentBy = c.Account?.AccountName ?? "Guest",
                CommentText = c.Content,
                CommentDate = c.CreatedAt
            }).ToList();
        }

        // Thêm mới comment
        public async Task<bool> AddAsync(int newsArticleId, string commentText, string commentBy)
        {
            try
            {
                // tìm account theo tên (nếu có)
                var account = await _context.SystemAccounts
                    .FirstOrDefaultAsync(a => a.AccountName == commentBy);

                var comment = new Comment
                {
                    NewsArticleID = newsArticleId,
                    AccountID = account?.AccountID ?? 0,
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

        // Xóa comment
        public async Task<bool> DeleteAsync(int commentId)
        {
            try
            {
                var comment = await _context.Comments.FindAsync(commentId);
                if (comment == null)
                    return false;

                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
