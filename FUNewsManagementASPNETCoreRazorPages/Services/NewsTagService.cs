using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface INewsTagService
    {
        Task<IEnumerable<NewsTag>> GetAllAsync();
        Task<IEnumerable<int>> GetTagIdsForNewsAsync(string newsId);
        Task<IEnumerable<NewsArticle>> GetArticlesByTagAsync(int tagId);
        Task<bool> AddAsync(string newsId, int tagId);
        Task<bool> DeleteAsync(string newsId, int tagId);
    }

    public class NewsTagService : INewsTagService
    {
        private readonly IGenericRepository<NewsTag> _repo;
        private readonly IGenericRepository<NewsArticle> _newsRepo;
        private readonly IGenericRepository<Tag> _tagRepo;

        public NewsTagService(
            IGenericRepository<NewsTag> repo,
            IGenericRepository<NewsArticle> newsRepo,
            IGenericRepository<Tag> tagRepo)
        {
            _repo = repo;
            _newsRepo = newsRepo;
            _tagRepo = tagRepo;
        }

        public async Task<IEnumerable<NewsTag>> GetAllAsync() => await _repo.GetAllAsync();

        public async Task<IEnumerable<int>> GetTagIdsForNewsAsync(string newsId)
        {
            var list = await _repo.GetByPredicateAsync(x => x.NewsArticleId == newsId);
            return list.Select(x => x.TagId);
        }

        public async Task<IEnumerable<NewsArticle>> GetArticlesByTagAsync(int tagId)
        {
            var tagLinks = await _repo.GetByPredicateAsync(x => x.TagId == tagId);
            var newsIds = tagLinks.Select(x => x.NewsArticleId).ToList();
            var allNews = await _newsRepo.GetAllAsync();
            return allNews.Where(n => newsIds.Contains(n.NewsArticleId));
        }

        public async Task<bool> AddAsync(string newsId, int tagId)
        {
            var exists = await _repo.ExistsAsync(x => x.NewsArticleId == newsId && x.TagId == tagId);
            if (exists) return false;

            var entity = new NewsTag { NewsArticleId = newsId, TagId = tagId };
            return await _repo.AddAsync(entity);
        }

        public async Task<bool> DeleteAsync(string newsId, int tagId)
        {
            var list = await _repo.GetByPredicateAsync(x => x.NewsArticleId == newsId && x.TagId == tagId);
            var entity = list.FirstOrDefault();
            if (entity == null) return false;

            return await _repo.DeleteAsync(new { newsId, tagId });
        }
    }
}
