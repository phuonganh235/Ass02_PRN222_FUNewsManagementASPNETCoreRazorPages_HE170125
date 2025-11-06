using DataAccess.Entities;

namespace DataAccess.Repositories
{
    public interface ITagRepository : IGenericRepository<Tag>
    {
        Task<bool> TagNameExistsAsync(string name, int? excludeTagId = null);
        Task<IEnumerable<Tag>> SearchTagsAsync(string? searchTerm);
        Task<bool> IsTagUsedAsync(int tagId);
        Task<IEnumerable<Tag>> GetTagsByNewsArticleAsync(string newsArticleId);
    }
}