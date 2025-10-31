using BusinessLogic.ViewModels;

namespace BusinessLogic.Interfaces
{
    public interface ICommentService
    {
        Task<List<CommentVM>> GetAllAsync();
        Task<List<CommentVM>> GetByNewsAsync(string newsArticleId);
        Task<bool> AddAsync(string newsArticleId, string commentText, string commentBy);
        Task<bool> DeleteAsync(int commentId);
    }
}
