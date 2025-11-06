using BusinessLogic.ViewModels;
using DataAccess.Entities;

namespace BusinessLogic.Interfaces
{
    public interface INewsArticleService
    {
        Task<NewsArticle?> GetAsync(string newsArticleId);
        Task<List<NewsArticleVM>> GetPagedAsync(
            int page, int pageSize,
            string? keyword = null,
            short? categoryId = null,
            bool? status = null,
            int? tagId = null);

        Task<NewsArticle> CreateAsync(
            string newsArticleId,
            string title,
            string? headline,
            string? content,
            string? source,
            short categoryId,
            bool status,
            short createdById,
            IEnumerable<int>? tagIds);

        Task<NewsArticle> UpdateAsync(
            string newsArticleId,
            string title,
            string? headline,
            string? content,
            string? source,
            short categoryId,
            bool status,
            short? updatedById,
            IEnumerable<int>? tagIds);

        Task<bool> DeleteAsync(string newsArticleId);
    }
}
