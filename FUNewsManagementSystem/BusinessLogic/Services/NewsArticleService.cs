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
                .Include(n => n.NewsArticleTags)
                    .ThenInclude(nt => nt.Tag)
                .ToListAsync();

            return data.Select(n => new NewsArticleVM
            {
                NewsArticleID = n.NewsArticleID,
                NewsTitle = n.NewsTitle,
                NewsContent = n.NewsContent,
                NewsSource = n.NewsSource,
                CategoryID = n.CategoryID,
                CategoryName = n.Category?.CategoryName,
                CreatedByID = n.CreatedByID,
                CreatedByName = n.CreatedBy?.AccountName,
                NewsStatus = n.NewsStatus,
                SelectedTagIDs = n.NewsArticleTags?.Select(x => x.TagID).ToList() ?? new List<int>(),
                SelectedTagNamesDisplay = string.Join(", ",
                    n.NewsArticleTags?.Select(x => x.Tag!.TagName) ?? new List<string>())
            }).ToList();
        }

        public async Task<NewsArticleVM?> GetByIdAsync(int id)
        {
            var n = await _ctx.NewsArticles
                .Include(x => x.Category)
                .Include(x => x.CreatedBy)
                .Include(x => x.NewsArticleTags)
                    .ThenInclude(nt => nt.Tag)
                .FirstOrDefaultAsync(x => x.NewsArticleID == id);

            if (n == null) return null;

            return new NewsArticleVM
            {
                NewsArticleID = n.NewsArticleID,
                NewsTitle = n.NewsTitle,
                NewsContent = n.NewsContent,
                NewsSource = n.NewsSource,
                CategoryID = n.CategoryID,
                CategoryName = n.Category?.CategoryName,
                CreatedByID = n.CreatedByID,
                CreatedByName = n.CreatedBy?.AccountName,
                NewsStatus = n.NewsStatus,
                SelectedTagIDs = n.NewsArticleTags?.Select(t => t.TagID).ToList() ?? new List<int>(),
                SelectedTagNamesDisplay = string.Join(", ",
                    n.NewsArticleTags?.Select(x => x.Tag!.TagName) ?? new List<string>())
            };
        }

        public async Task<bool> CreateAsync(
            string newsTitle,
            string newsContent,
            string newsSource,
            int categoryId,
            int createdById,
            int newsStatus,
            List<int> tagIds
        )
        {
            var news = new NewsArticle
            {
                NewsTitle = newsTitle,
                NewsContent = newsContent,
                NewsSource = newsSource,
                CategoryID = categoryId,
                CreatedByID = createdById,
                NewsStatus = newsStatus
            };

            _ctx.NewsArticles.Add(news);
            await _ctx.SaveChangesAsync();

            // create many-to-many tags
            foreach (var tid in tagIds.Distinct())
            {
                _ctx.NewsArticleTags.Add(new NewsArticleTag
                {
                    NewsArticleID = news.NewsArticleID,
                    TagID = tid
                });
            }

            await _ctx.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(
            int newsId,
            string newsTitle,
            string newsContent,
            string newsSource,
            int categoryId,
            int createdById,
            int newsStatus,
            List<int> tagIds
        )
        {
            var news = await _ctx.NewsArticles
                .Include(n => n.NewsArticleTags)
                .FirstOrDefaultAsync(n => n.NewsArticleID == newsId);

            if (news == null) return false;

            news.NewsTitle = newsTitle;
            news.NewsContent = newsContent;
            news.NewsSource = newsSource;
            news.CategoryID = categoryId;
            news.CreatedByID = createdById;
            news.NewsStatus = newsStatus;

            // update tag mapping = remove all then add new
            _ctx.NewsArticleTags.RemoveRange(news.NewsArticleTags);

            foreach (var tid in tagIds.Distinct())
            {
                _ctx.NewsArticleTags.Add(new NewsArticleTag
                {
                    NewsArticleID = news.NewsArticleID,
                    TagID = tid
                });
            }

            _ctx.NewsArticles.Update(news);
            await _ctx.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var news = await _ctx.NewsArticles
                .Include(n => n.NewsArticleTags)
                .FirstOrDefaultAsync(n => n.NewsArticleID == id);

            if (news == null) return false;

            _ctx.NewsArticleTags.RemoveRange(news.NewsArticleTags);
            _ctx.NewsArticles.Remove(news);
            await _ctx.SaveChangesAsync();
            return true;
        }
    }
}
