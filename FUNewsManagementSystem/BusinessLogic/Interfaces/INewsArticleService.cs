using BusinessLogic.ViewModels;

namespace BusinessLogic.Interfaces
{
    public interface INewsArticleService
    {
        Task<IEnumerable<NewsArticleViewModel>> GetAllNewsAsync();
        Task<IEnumerable<NewsArticleViewModel>> GetActiveNewsAsync();
        Task<IEnumerable<NewsArticleViewModel>> GetNewsByAuthorAsync(short authorId);
        Task<IEnumerable<NewsArticleViewModel>> GetNewsByCategoryAsync(short categoryId);
        Task<NewsArticleViewModel?> GetNewsByIdAsync(string newsId);
        Task<bool> CreateNewsAsync(NewsArticleViewModel model, short createdById);
        Task<bool> UpdateNewsAsync(NewsArticleViewModel model, short updatedById);
        Task<bool> DeleteNewsAsync(string newsId);
        Task<IEnumerable<NewsArticleViewModel>> SearchNewsAsync(string? searchTerm, short? categoryId, bool? status, DateTime? fromDate, DateTime? toDate);
        Task<IEnumerable<NewsArticleViewModel>> GetRelatedNewsAsync(string newsId, int count = 5);
        Task<bool> NewsIdExistsAsync(string newsId);
        Task<Dictionary<string, int>> GetArticleStatisticsByCategoryAsync(DateTime? startDate, DateTime? endDate);
        Task<Dictionary<string, int>> GetArticleStatisticsByAuthorAsync(DateTime? startDate, DateTime? endDate);
    }
}