using BusinessObjects;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface INewsService
    {
        Task<IEnumerable<NewsArticle>> GetAllAsync();
        Task<NewsArticle?> GetByIdAsync(string id);
        Task<bool> AddAsync(NewsArticle article);
        Task<bool> UpdateAsync(NewsArticle article);
        Task<bool> DeleteAsync(string id);
        Task<IEnumerable<NewsArticle>> SearchByStaffAsync(string staffName, string? keyword = null);
        Task<NewsArticle?> DuplicateAsync(string id, short createdById);
    }

    public class NewsService : INewsService
    {
        private readonly IGenericRepository<NewsArticle> _repo;
        private readonly ILogger<NewsService> _logger;

        public NewsService(IGenericRepository<NewsArticle> repo, ILogger<NewsService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<IEnumerable<NewsArticle>> GetAllAsync() => await _repo.GetAllAsync();

        public async Task<NewsArticle?> GetByIdAsync(string id)
        {
            var list = await _repo.GetByPredicateAsync(x => x.NewsArticleId == id);
            return list.FirstOrDefault();
        }

        public async Task<bool> AddAsync(NewsArticle article)
        {
            article.CreatedDate = DateTime.Now;
            return await _repo.AddAsync(article);
        }

        public async Task<bool> UpdateAsync(NewsArticle article)
        {
            article.ModifiedDate = DateTime.Now;
            return await _repo.UpdateAsync(article);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            return await _repo.DeleteAsync(id);
        }

        public async Task<IEnumerable<NewsArticle>> SearchByStaffAsync(string staffName, string? keyword = null)
        {
            return await _repo.GetByPredicateAsync(x =>
                (x.CreatedBy != null && x.CreatedBy.AccountName.Contains(staffName)) &&
                (string.IsNullOrEmpty(keyword) ||
                 (x.NewsTitle.Contains(keyword) || x.NewsContent.Contains(keyword))));
        }

        public async Task<NewsArticle?> DuplicateAsync(string id, short createdById)
        {
            var article = await GetByIdAsync(id);
            if (article == null) return null;

            var copy = new NewsArticle
            {
                NewsArticleId = Guid.NewGuid().ToString().Substring(0, 10),
                NewsTitle = article.NewsTitle + " (Copy)",
                Headline = article.Headline,
                NewsContent = article.NewsContent,
                NewsSource = article.NewsSource,
                CategoryId = article.CategoryId,
                CreatedDate = DateTime.Now,
                CreatedById = createdById,
                NewsStatus = false
            };
            await _repo.AddAsync(copy);
            return copy;
        }
    }
}
