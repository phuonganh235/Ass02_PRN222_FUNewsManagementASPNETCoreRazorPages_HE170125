using DataAccess.Context;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class NewsArticleRepository : GenericRepository<NewsArticle>, INewsArticleRepository
    {
        public NewsArticleRepository(FUNewsManagementDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<NewsArticle>> GetActiveNewsAsync()
        {
            return await _dbSet
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Include(n => n.NewsTags)
                    .ThenInclude(nt => nt.Tag)
                .Where(n => n.NewsStatus)
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<NewsArticle>> GetNewsByAuthorAsync(short authorId)
        {
            return await _dbSet
                .Include(n => n.Category)
                .Include(n => n.NewsTags)
                    .ThenInclude(nt => nt.Tag)
                .Where(n => n.CreatedById == authorId)
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<NewsArticle>> GetNewsByCategoryAsync(short categoryId)
        {
            return await _dbSet
                .Include(n => n.CreatedBy)
                .Include(n => n.NewsTags)
                    .ThenInclude(nt => nt.Tag)
                .Where(n => n.CategoryId == categoryId && n.NewsStatus)
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync();
        }

        public async Task<NewsArticle?> GetNewsWithDetailsAsync(string newsId)
        {
            return await _dbSet
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Include(n => n.UpdatedBy)
                .Include(n => n.NewsTags)
                    .ThenInclude(nt => nt.Tag)
                .Include(n => n.Comments)
                    .ThenInclude(c => c.Account)
                .FirstOrDefaultAsync(n => n.NewsArticleId == newsId);
        }

        public async Task<IEnumerable<NewsArticle>> SearchNewsAsync(
            string? searchTerm,
            short? categoryId,
            bool? status,
            DateTime? fromDate,
            DateTime? toDate)
        {
            var query = _dbSet
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(n =>
                    n.NewsTitle.ToLower().Contains(searchTerm) ||
                    n.Headline.ToLower().Contains(searchTerm) ||
                    n.NewsContent.ToLower().Contains(searchTerm));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(n => n.CategoryId == categoryId.Value);
            }

            if (status.HasValue)
            {
                query = query.Where(n => n.NewsStatus == status.Value);
            }

            if (fromDate.HasValue)
            {
                query = query.Where(n => n.CreatedDate >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(n => n.CreatedDate <= toDate.Value);
            }

            return await query
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<NewsArticle>> GetRelatedNewsAsync(string newsId, int count = 5)
        {
            var currentNews = await _dbSet
                .Include(n => n.NewsTags)
                .FirstOrDefaultAsync(n => n.NewsArticleId == newsId);

            if (currentNews == null)
                return new List<NewsArticle>();

            var tagIds = currentNews.NewsTags.Select(nt => nt.TagId).ToList();

            return await _dbSet
                .Include(n => n.Category)
                .Include(n => n.NewsTags)
                    .ThenInclude(nt => nt.Tag)
                .Where(n =>
                    n.NewsArticleId != newsId &&
                    n.NewsStatus &&
                    (n.CategoryId == currentNews.CategoryId ||
                     n.NewsTags.Any(nt => tagIds.Contains(nt.TagId))))
                .OrderByDescending(n => n.CreatedDate)
                .Take(count)
                .ToListAsync();
        }

        public async Task<bool> NewsIdExistsAsync(string newsId)
        {
            return await _dbSet.AnyAsync(n => n.NewsArticleId == newsId);
        }

        public async Task<Dictionary<short, int>> GetArticleCountByCategoryAsync(DateTime? startDate, DateTime? endDate)
        {
            var query = _dbSet.AsQueryable();

            if (startDate.HasValue)
            {
                query = query.Where(n => n.CreatedDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(n => n.CreatedDate <= endDate.Value);
            }

            return await query
                .GroupBy(n => n.CategoryId)
                .Select(g => new { CategoryId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.CategoryId, x => x.Count);
        }

        public async Task<Dictionary<short, int>> GetArticleCountByAuthorAsync(DateTime? startDate, DateTime? endDate)
        {
            var query = _dbSet.AsQueryable();

            if (startDate.HasValue)
            {
                query = query.Where(n => n.CreatedDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(n => n.CreatedDate <= endDate.Value);
            }

            return await query
                .GroupBy(n => n.CreatedById)
                .Select(g => new { AuthorId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.AuthorId, x => x.Count);
        }
    }
}