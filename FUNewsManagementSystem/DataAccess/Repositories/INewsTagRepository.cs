using DataAccess.Entities;

namespace DataAccess.Repositories
{
    public interface INewsTagRepository : IGenericRepository<NewsTag>
    {
        Task RemoveTagsFromNewsAsync(string newsArticleId);
        Task AddTagsToNewsAsync(string newsArticleId, List<int> tagIds);
        Task UpdateNewsTagsAsync(string newsArticleId, List<int> tagIds);
        Task<IEnumerable<NewsTag>> GetByNewsArticleIdAsync(string newsArticleId);
    }
}