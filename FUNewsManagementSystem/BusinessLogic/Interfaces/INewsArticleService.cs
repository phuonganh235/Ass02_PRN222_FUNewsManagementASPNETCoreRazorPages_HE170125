using BusinessLogic.ViewModels;

namespace BusinessLogic.Interfaces
{
    public interface INewsArticleService
    {
        Task<List<NewsArticleVM>> GetAllAsync();
        Task<NewsArticleVM?> GetByIdAsync(int id);

        Task<bool> CreateAsync(
            string newsTitle,
            string newsContent,
            string newsSource,
            int categoryId,
            int createdById,
            int newsStatus,
            List<int> tagIds
        );

        Task<bool> UpdateAsync(
            int newsId,
            string newsTitle,
            string newsContent,
            string newsSource,
            int categoryId,
            int createdById,
            int newsStatus,
            List<int> tagIds
        );

        Task<bool> DeleteAsync(int id);
    }
}
