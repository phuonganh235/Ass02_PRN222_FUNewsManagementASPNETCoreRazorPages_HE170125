using BusinessLogic.Interfaces;
using BusinessLogic.ViewModels;
using DataAccess.Repositories;

namespace BusinessLogic.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly INewsArticleRepository _newsRepository;
        private readonly ISystemAccountRepository _accountRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITagRepository _tagRepository;
        private readonly ICommentRepository _commentRepository;

        public DashboardService(
            INewsArticleRepository newsRepository,
            ISystemAccountRepository accountRepository,
            ICategoryRepository categoryRepository,
            ITagRepository tagRepository,
            ICommentRepository commentRepository)
        {
            _newsRepository = newsRepository;
            _accountRepository = accountRepository;
            _categoryRepository = categoryRepository;
            _tagRepository = tagRepository;
            _commentRepository = commentRepository;
        }

        public async Task<DashboardViewModel> GetDashboardDataAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            var dashboard = new DashboardViewModel();

            // Get counts
            dashboard.TotalArticles = await _newsRepository.CountAsync();
            dashboard.ActiveArticles = await _newsRepository.CountAsync(n => n.NewsStatus);
            dashboard.InactiveArticles = await _newsRepository.CountAsync(n => !n.NewsStatus);
            dashboard.TotalAccounts = await _accountRepository.CountAsync();
            dashboard.TotalCategories = await _categoryRepository.CountAsync();
            dashboard.TotalTags = await _tagRepository.CountAsync();
            dashboard.TotalComments = await _commentRepository.GetTotalCommentsCountAsync();

            // Get statistics by category
            var categoryStats = await _newsRepository.GetArticleCountByCategoryAsync(startDate, endDate);
            foreach (var stat in categoryStats)
            {
                var category = await _categoryRepository.GetByIdAsync(stat.Key);
                if (category != null)
                {
                    dashboard.ArticlesByCategory[category.CategoryName] = stat.Value;
                }
            }

            // Get statistics by author
            var authorStats = await _newsRepository.GetArticleCountByAuthorAsync(startDate, endDate);
            foreach (var stat in authorStats)
            {
                var author = await _accountRepository.GetByIdAsync(stat.Key);
                if (author != null)
                {
                    dashboard.ArticlesByAuthor[author.AccountName] = stat.Value;
                }
            }

            // Get recent articles
            var recentNews = await _newsRepository.GetActiveNewsAsync();
            dashboard.RecentArticles = recentNews.Take(5).Select(n => new NewsArticleViewModel
            {
                NewsArticleId = n.NewsArticleId,
                NewsTitle = n.NewsTitle,
                Headline = n.Headline,
                CreatedDate = n.CreatedDate,
                CreatedByName = n.CreatedBy?.AccountName,
                CategoryName = n.Category?.CategoryName
            }).ToList();

            return dashboard;
        }
    }
}