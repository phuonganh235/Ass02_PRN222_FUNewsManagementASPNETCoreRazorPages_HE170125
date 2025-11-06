using BusinessLogic.Interfaces;
using BusinessLogic.ViewModels;
using DataAccess.Context;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public class NewsArticleService : INewsArticleService
    {
        private readonly FUNewsDbContext _ctx;
        public NewsArticleService(FUNewsDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<NewsArticle?> GetAsync(string newsArticleId)
        {
            return await _ctx.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.NewsTags).ThenInclude(nt => nt.Tag)
                .FirstOrDefaultAsync(n => n.NewsArticleID == newsArticleId);
        }

        public async Task<List<NewsArticleVM>> GetPagedAsync(
            int page, int pageSize,
            string? keyword = null,
            short? categoryId = null,
            bool? status = null,
            int? tagId = null)
        {
            var q = _ctx.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.NewsTags).ThenInclude(nt => nt.Tag)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
                q = q.Where(n => n.NewsTitle.Contains(keyword) || (n.Headline ?? "").Contains(keyword));

            if (categoryId.HasValue)
                q = q.Where(n => n.CategoryID == categoryId.Value);

            if (status.HasValue)
                q = q.Where(n => n.NewsStatus == status.Value);

            if (tagId.HasValue)
                q = q.Where(n => n.NewsTags.Any(t => t.TagID == tagId.Value));

            q = q.OrderByDescending(n => n.CreatedDate);

            var items = await q.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return items.Select(n => new NewsArticleVM
            {
                NewsArticleID = n.NewsArticleID,
                NewsTitle = n.NewsTitle,
                Headline = n.Headline,
                CreatedDate = n.CreatedDate,
                CategoryName = n.Category?.CategoryName,
                NewsStatus = n.NewsStatus,
                SelectedTagIds = n.NewsTags.Select(t => t.TagID).ToList(),
                SelectedTagNamesDisplay = string.Join(", ", n.NewsTags.Where(t => t.Tag != null).Select(t => t.Tag!.TagName))
            }).ToList();
        }

        public async Task<NewsArticle> CreateAsync(
            string newsArticleId, string title, string? headline, string? content, string? source,
            short categoryId, bool status, short createdById, IEnumerable<int>? tagIds)
        {
            var entity = new NewsArticle
            {
                NewsArticleID = newsArticleId,
                NewsTitle = title,
                Headline = headline,
                NewsContent = content,
                NewsSource = source,
                CategoryID = categoryId,
                NewsStatus = status,
                CreatedByID = createdById,
                CreatedDate = DateTime.UtcNow
            };

            if (tagIds != null)
                foreach (var id in tagIds.Distinct())
                    entity.NewsTags.Add(new NewsTag { NewsArticleID = newsArticleId, TagID = id });

            _ctx.NewsArticles.Add(entity);
            await _ctx.SaveChangesAsync();
            return entity;
        }

        public async Task<NewsArticle> UpdateAsync(
            string newsArticleId, string title, string? headline, string? content, string? source,
            short categoryId, bool status, short? updatedById, IEnumerable<int>? tagIds)
        {
            var entity = await _ctx.NewsArticles
                .Include(n => n.NewsTags)
                .FirstOrDefaultAsync(n => n.NewsArticleID == newsArticleId)
                ?? throw new KeyNotFoundException("NewsArticle not found");

            entity.NewsTitle = title;
            entity.Headline = headline;
            entity.NewsContent = content;
            entity.NewsSource = source;
            entity.CategoryID = categoryId;
            entity.NewsStatus = status;
            entity.UpdatedByID = updatedById;
            entity.ModifiedDate = DateTime.UtcNow;

            // update tags
            var newTags = tagIds?.Distinct().ToHashSet() ?? new HashSet<int>();
            var oldTags = entity.NewsTags.Select(t => t.TagID).ToHashSet();

            var toRemove = entity.NewsTags.Where(t => !newTags.Contains(t.TagID)).ToList();
            if (toRemove.Count > 0) _ctx.NewsTags.RemoveRange(toRemove);

            var toAdd = newTags.Where(id => !oldTags.Contains(id))
                               .Select(id => new NewsTag { NewsArticleID = newsArticleId, TagID = id }).ToList();
            if (toAdd.Count > 0) _ctx.NewsTags.AddRange(toAdd);

            await _ctx.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(string newsArticleId)
        {
            var entity = await _ctx.NewsArticles.FirstOrDefaultAsync(n => n.NewsArticleID == newsArticleId);
            if (entity == null) return false;
            _ctx.NewsArticles.Remove(entity);
            await _ctx.SaveChangesAsync();
            return true;
        }
    }
}
