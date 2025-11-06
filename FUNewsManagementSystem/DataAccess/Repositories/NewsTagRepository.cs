using DataAccess.Context;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class NewsTagRepository : GenericRepository<NewsTag>, INewsTagRepository
    {
        public NewsTagRepository(FUNewsManagementDbContext context) : base(context)
        {
        }

        public async Task RemoveTagsFromNewsAsync(string newsArticleId)
        {
            var existingTags = await _dbSet
                .Where(nt => nt.NewsArticleId == newsArticleId)
                .ToListAsync();

            if (existingTags.Any())
            {
                _dbSet.RemoveRange(existingTags);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddTagsToNewsAsync(string newsArticleId, List<int> tagIds)
        {
            var newsTags = tagIds.Select(tagId => new NewsTag
            {
                NewsArticleId = newsArticleId,
                TagId = tagId
            }).ToList();

            await _dbSet.AddRangeAsync(newsTags);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateNewsTagsAsync(string newsArticleId, List<int> tagIds)
        {
            // Remove existing tags
            await RemoveTagsFromNewsAsync(newsArticleId);

            // Add new tags
            if (tagIds.Any())
            {
                await AddTagsToNewsAsync(newsArticleId, tagIds);
            }
        }

        public async Task<IEnumerable<NewsTag>> GetByNewsArticleIdAsync(string newsArticleId)
        {
            return await _dbSet
                .Include(nt => nt.Tag)
                .Where(nt => nt.NewsArticleId == newsArticleId)
                .ToListAsync();
        }
    }
}