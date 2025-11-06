using DataAccess.Entities;

namespace DataAccess.Repositories
{
    public interface INewsArticleRepository : IGenericRepository<NewsArticle>
    {
        Task<IEnumerable<NewsArticle>> GetActiveNewsAsync();
        Task<IEnumerable<NewsArticle>> GetNewsByAuthorAsync(short authorId);
        Task<IEnumerable<NewsArticle>> GetNewsByCategoryAsync(short categoryId);
        Task<NewsArticle?> GetNewsWithDetailsAsync(string newsId);
        Task<IEnumerable<NewsArticle>> SearchNewsAsync(string? searchTerm, short? categoryId, bool? status, DateTime? fromDate, DateTime? toDate);
        Task<IEnumerable<NewsArticle>> GetRelatedNewsAsync(string newsId, int count = 5);
        Task<bool> NewsIdExistsAsync(string newsId);
        Task<Dictionary<short, int>> GetArticleCountByCategoryAsync(DateTime? startDate, DateTime? endDate);
        Task<Dictionary<short, int>> GetArticleCountByAuthorAsync(DateTime? startDate, DateTime? endDate);
    }
}