using BusinessLogic.ViewModels;

namespace BusinessLogic.Interfaces
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentViewModel>> GetCommentsByNewsArticleAsync(string newsArticleId);
        Task<CommentViewModel?> GetCommentByIdAsync(int id);
        Task<CommentViewModel> CreateCommentAsync(string newsArticleId, short accountId, string content);
        Task<bool> DeleteCommentAsync(int id);
        Task<int> GetCommentCountByNewsArticleAsync(string newsArticleId);
        Task<int> GetTotalCommentsCountAsync();
    }
}