using BusinessLogic.ViewModels;
using DataAccess.Entities;

namespace BusinessLogic.Interfaces
{
    public interface ICommentService
    {
        Task<List<CommentVM>> GetByNewsAsync(string newsArticleId);
        Task<Comment> AddAsync(string newsArticleId, short accountId, string content);
    }
}
