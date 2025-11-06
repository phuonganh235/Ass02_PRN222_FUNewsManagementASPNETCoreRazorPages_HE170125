using DataAccess.Context;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        public CommentRepository(FUNewsManagementDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Comment>> GetCommentsByNewsArticleAsync(string newsArticleId)
        {
            return await _dbSet
                .Include(c => c.Account)
                .Where(c => c.NewsArticleId == newsArticleId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<int> GetCommentCountByNewsArticleAsync(string newsArticleId)
        {
            return await _dbSet.CountAsync(c => c.NewsArticleId == newsArticleId);
        }

        public async Task<int> GetTotalCommentsCountAsync()
        {
            return await _dbSet.CountAsync();
        }
    }
}