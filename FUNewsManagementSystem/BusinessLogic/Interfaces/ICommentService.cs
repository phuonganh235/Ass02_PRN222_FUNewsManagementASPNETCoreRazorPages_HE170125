using BusinessLogic.ViewModels;

namespace BusinessLogic.Interfaces
{
    public interface ICommentService
    {
        Task<List<CommentVM>> GetAllAsync();
        Task<List<CommentVM>> GetByNewsAsync(int newsArticleId);
        Task<bool> AddAsync(int newsArticleId, string commentText, string commentBy);
        Task<bool> DeleteAsync(int commentId);
    }
}
