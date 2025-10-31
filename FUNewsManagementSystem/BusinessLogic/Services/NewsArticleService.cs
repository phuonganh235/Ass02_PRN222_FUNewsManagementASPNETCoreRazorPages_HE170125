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

        public async Task<List<NewsArticleVM>> GetAllAsync()
        {
            var data = await _ctx.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Include(n => n.NewsTags!)
                    .ThenInclude(nt => nt.Tag)
                .ToListAsync();

            return data.Select(n => new NewsArticleVM
            {
                NewsArticleID = n.NewsArticleID,
                NewsTitle = n.NewsTitle,
                Headline = n.Headline,
                NewsContent = n.NewsContent,
                NewsSource = n.NewsSource,
                CategoryID = n.CategoryID,
                CategoryName = n.Category?.CategoryName,
                CreatedByID = n.CreatedByID,
                CreatedByName = n.CreatedBy?.AccountName,
                CreatedDate = n.CreatedDate,
                ModifiedDate = n.ModifiedDate,
                NewsStatus = n.NewsStatus,
                SelectedTagIDs = n.NewsTags?.Select(t => t.TagID).ToList() ?? new(),
                SelectedTagNames = n.NewsTags?.Select(t => t.Tag!.TagName ?? "").ToList() ?? new()
            }).ToList();
        }

        public async Task<NewsArticleVM?> GetByIdAsync(string id)
        {
            var n = await _ctx.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Include(n => n.NewsTags!)
                    .ThenInclude(nt => nt.Tag)
                .FirstOrDefaultAsync(n => n.NewsArticleID == id);

            if (n == null) return null;

            return new NewsArticleVM
            {
                NewsArticleID = n.NewsArticleID,
                NewsTitle = n.NewsTitle,
                Headline = n.Headline,
                NewsContent = n.NewsContent,
                NewsSource = n.NewsSource,
                CategoryID = n.CategoryID,
                CategoryName = n.Category?.CategoryName,
                CreatedByID = n.CreatedByID,
                CreatedByName = n.CreatedBy?.AccountName,
                CreatedDate = n.CreatedDate,
                ModifiedDate = n.ModifiedDate,
                NewsStatus = n.NewsStatus,
                SelectedTagIDs = n.NewsTags?.Select(t => t.TagID).ToList() ?? new(),
                SelectedTagNames = n.NewsTags?.Select(t => t.Tag!.TagName ?? "").ToList() ?? new()
            };
        }

        public async Task<bool> CreateAsync(
            string newsTitle,
            string headline,
            string newsContent,
            string newsSource,
            short categoryId,
            short createdById,
            bool newsStatus,
            List<int> tagIds)
        {
            var news = new NewsArticle
            {
                NewsArticleID = Guid.NewGuid().ToString("N").Substring(0, 20),
                NewsTitle = newsTitle,
                Headline = headline,
                NewsContent = newsContent,
                NewsSource = newsSource,
                CategoryID = categoryId,
                CreatedByID = createdById,
                NewsStatus = newsStatus,
                CreatedDate = DateTime.Now
            };

            _ctx.NewsArticles.Add(news);
            await _ctx.SaveChangesAsync();

            foreach (var tid in tagIds.Distinct())
            {
                _ctx.NewsTags.Add(new NewsTag
                {
                    NewsArticleID = news.NewsArticleID,
                    TagID = tid
                });
            }

            await _ctx.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(
            string newsArticleId,
            string newsTitle,
            string headline,
            string newsContent,
            string newsSource,
            short categoryId,
            bool newsStatus,
            List<int> tagIds)
        {
            var news = await _ctx.NewsArticles
                .Include(n => n.NewsTags)
                .FirstOrDefaultAsync(n => n.NewsArticleID == newsArticleId);

            if (news == null) return false;

            news.NewsTitle = newsTitle;
            news.Headline = headline;
            news.NewsContent = newsContent;
            news.NewsSource = newsSource;
            news.CategoryID = categoryId;
            news.NewsStatus = newsStatus;
            news.ModifiedDate = DateTime.Now;

            _ctx.NewsTags.RemoveRange(news.NewsTags ?? new List<NewsTag>());

            foreach (var tid in tagIds.Distinct())
            {
                _ctx.NewsTags.Add(new NewsTag
                {
                    NewsArticleID = newsArticleId,
                    TagID = tid
                });
            }

            await _ctx.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var news = await _ctx.NewsArticles
                .Include(n => n.NewsTags)
                .FirstOrDefaultAsync(n => n.NewsArticleID == id);

            if (news == null) return false;

            _ctx.NewsTags.RemoveRange(news.NewsTags ?? new List<NewsTag>());
            _ctx.NewsArticles.Remove(news);

            await _ctx.SaveChangesAsync();
            return true;
        }
    }
}
