using BusinessLogic.ViewModels;

namespace BusinessLogic.Interfaces
{
    public interface INewsArticleService
    {
        Task<List<NewsArticleVM>> GetAllAsync();
        Task<NewsArticleVM?> GetByIdAsync(string id);

        Task<bool> CreateAsync(
            string newsTitle,
            string headline,
            string newsContent,
            string newsSource,
            short categoryId,
            short createdById,
            bool newsStatus,
            List<int> tagIds
        );

        Task<bool> UpdateAsync(
            string newsArticleId,
            string newsTitle,
            string headline,
            string newsContent,
            string newsSource,
            short categoryId,
            bool newsStatus,
            List<int> tagIds
        );

        Task<bool> DeleteAsync(string id);
    }
}
