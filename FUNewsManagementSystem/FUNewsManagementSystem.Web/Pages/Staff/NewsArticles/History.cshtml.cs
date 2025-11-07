using BusinessLogic.Interfaces;
using BusinessLogic.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.Web.Pages.Staff.NewsArticles
{
    public class HistoryModel : PageModel
    {
        private readonly INewsArticleService _newsService;
        private readonly ICommentService _commentService;

        public List<NewsArticleViewModel> Articles { get; set; } = new List<NewsArticleViewModel>();

        [BindProperty(SupportsGet = true)]
        public DateTime? FromDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? ToDate { get; set; }

        public int TotalArticles { get; set; }
        public int ActiveArticles { get; set; }
        public int InactiveArticles { get; set; }
        public int TotalComments { get; set; }

        public HistoryModel(INewsArticleService newsService, ICommentService commentService)
        {
            _newsService = newsService;
            _commentService = commentService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            // Check if user is Staff
            var userRole = HttpContext.Session.GetInt32("UserRole");
            if (userRole != 1)
            {
                return RedirectToPage("/Index");
            }

            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return RedirectToPage("/Login");
            }

            // Get all articles by current user
            var allArticles = await _newsService.GetNewsByAuthorAsync((short)userId.Value);

            // Filter by date if specified
            if (FromDate.HasValue)
            {
                allArticles = allArticles.Where(a => a.CreatedDate.Date >= FromDate.Value.Date);
            }

            if (ToDate.HasValue)
            {
                allArticles = allArticles.Where(a => a.CreatedDate.Date <= ToDate.Value.Date);
            }

            Articles = allArticles.OrderByDescending(a => a.CreatedDate).ToList();

            // Calculate statistics
            TotalArticles = Articles.Count;
            ActiveArticles = Articles.Count(a => a.NewsStatus);
            InactiveArticles = Articles.Count(a => !a.NewsStatus);

            // Count total comments
            TotalComments = 0;
            foreach (var article in Articles)
            {
                TotalComments += await _commentService.GetCommentCountByNewsArticleAsync(article.NewsArticleId);
            }

            return Page();
        }
    }
}