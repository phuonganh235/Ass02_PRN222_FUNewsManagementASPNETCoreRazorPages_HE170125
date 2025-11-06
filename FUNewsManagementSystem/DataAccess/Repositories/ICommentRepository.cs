using DataAccess.Entities;

namespace DataAccess.Repositories
{
    public interface ICommentRepository : IGenericRepository<Comment>
    {
        Task<IEnumerable<Comment>> GetCommentsByNewsArticleAsync(string newsArticleId);
        Task<int> GetCommentCountByNewsArticleAsync(string newsArticleId);
        Task<int> GetTotalCommentsCountAsync();
    }
}