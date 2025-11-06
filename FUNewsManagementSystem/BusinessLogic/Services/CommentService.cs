using BusinessLogic.Interfaces;
using BusinessLogic.ViewModels;
using DataAccess.Context;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public class CommentService : ICommentService
    {
        private readonly FUNewsDbContext _ctx;
        public CommentService(FUNewsDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<List<CommentVM>> GetByNewsAsync(string newsArticleId)
        {
            return await _ctx.Comments
                .Include(c => c.Account)
                .Where(c => c.NewsArticleID == newsArticleId)
                .OrderByDescending(c => c.CreatedAt)
                .Select(c => new CommentVM
                {
                    CommentID = c.CommentID,
                    NewsArticleID = c.NewsArticleID,
                    AccountName = c.Account != null ? c.Account.AccountName : "(unknown)",
                    CommentDate = c.CreatedAt,
                    CommentText = c.Content
                })
                .ToListAsync();
        }

        public async Task<Comment> AddAsync(string newsArticleId, short accountId, string content)
        {
            var c = new Comment
            {
                NewsArticleID = newsArticleId,
                AccountID = accountId,
                Content = content,
                CreatedAt = DateTime.UtcNow
            };
            _ctx.Comments.Add(c);
            await _ctx.SaveChangesAsync();
            return c;
        }
    }
}
