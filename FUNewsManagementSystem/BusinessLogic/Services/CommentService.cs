using BusinessLogic.Interfaces;
using BusinessLogic.ViewModels;
using DataAccess.Entities;
using DataAccess.Repositories;

namespace BusinessLogic.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;

        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<IEnumerable<CommentViewModel>> GetCommentsByNewsArticleAsync(string newsArticleId)
        {
            var comments = await _commentRepository.GetCommentsByNewsArticleAsync(newsArticleId);
            return comments.Select(MapToViewModel);
        }

        public async Task<IEnumerable<CommentViewModel>> GetAllCommentsAsync()
        {
            var comments = await _commentRepository.GetAllCommentsWithDetailsAsync();
            return comments.Select(MapToViewModel);
        }

        public async Task<CommentViewModel?> GetCommentByIdAsync(int id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            return comment != null ? MapToViewModel(comment) : null;
        }

        public async Task<CommentViewModel> CreateCommentAsync(string newsArticleId, short accountId, string content)
        {
            var comment = new Comment
            {
                NewsArticleId = newsArticleId,
                AccountId = accountId,
                Content = content,
                CreatedAt = DateTime.Now
            };

            await _commentRepository.AddAsync(comment);

            // Reload to get related data
            var savedComment = await _commentRepository.GetByIdAsync(comment.CommentId);
            return MapToViewModel(savedComment!);
        }

        public async Task<bool> DeleteCommentAsync(int id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            if (comment == null) return false;

            await _commentRepository.DeleteAsync(comment);
            return true;
        }

        public async Task<int> GetCommentCountByNewsArticleAsync(string newsArticleId)
        {
            return await _commentRepository.GetCommentCountByNewsArticleAsync(newsArticleId);
        }

        public async Task<int> GetTotalCommentsCountAsync()
        {
            return await _commentRepository.GetTotalCommentsCountAsync();
        }

        private CommentViewModel MapToViewModel(Comment comment)
        {
            return new CommentViewModel
            {
                CommentId = comment.CommentId,
                NewsArticleId = comment.NewsArticleId,
                AccountId = comment.AccountId,
                AccountName = comment.Account?.AccountName ?? "Unknown",
                NewsTitle = comment.NewsArticle?.NewsTitle ?? "Unknown Article",
                Content = comment.Content,
                CreatedAt = comment.CreatedAt
            };
        }
    }
}