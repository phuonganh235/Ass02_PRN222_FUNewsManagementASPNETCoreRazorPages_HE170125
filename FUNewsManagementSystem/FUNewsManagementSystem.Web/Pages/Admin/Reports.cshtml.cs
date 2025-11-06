using BusinessLogic.Interfaces;
using BusinessLogic.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.Web.Pages.Admin
{
    public class ReportsModel : PageModel
    {
        private readonly INewsArticleService _newsService;
        private readonly ICommentService _commentService;

        [BindProperty(SupportsGet = true)]
        public DateTime? StartDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? EndDate { get; set; }

        public int TotalArticles { get; set; }
        public int ActiveArticles { get; set; }
        public int InactiveArticles { get; set; }
        public int TotalComments { get; set; }

        public Dictionary<string, int> ArticlesByCategory { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> ArticlesByAuthor { get; set; } = new Dictionary<string, int>();
        public List<NewsArticleViewModel> Articles { get; set; } = new List<NewsArticleViewModel>();

        public ReportsModel(INewsArticleService newsService, ICommentService commentService)
        {
            _newsService = newsService;
            _commentService = commentService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            // Check if user is Admin
            var userRole = HttpContext.Session.GetInt32("UserRole");
            if (userRole != 0)
            {
                return RedirectToPage("/Index");
            }

            // Get all articles with date filter
            var allArticles = await _newsService.SearchNewsAsync(
                null,
                null,
                null,
                StartDate,
                EndDate
            );

            Articles = allArticles.OrderByDescending(a => a.CreatedDate).ToList();

            // Calculate statistics
            TotalArticles = Articles.Count;
            ActiveArticles = Articles.Count(a => a.NewsStatus);
            InactiveArticles = Articles.Count(a => !a.NewsStatus);
            TotalComments = await _commentService.GetTotalCommentsCountAsync();

            // Get statistics by category
            ArticlesByCategory = await _newsService.GetArticleStatisticsByCategoryAsync(StartDate, EndDate);

            // Get statistics by author
            ArticlesByAuthor = await _newsService.GetArticleStatisticsByAuthorAsync(StartDate, EndDate);

            return Page();
        }
    }
}