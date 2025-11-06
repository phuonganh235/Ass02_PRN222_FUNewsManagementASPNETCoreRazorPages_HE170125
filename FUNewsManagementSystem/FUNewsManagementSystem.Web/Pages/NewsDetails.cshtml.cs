using BusinessLogic.Interfaces;
using BusinessLogic.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.Web.Pages
{
    public class NewsDetailsModel : PageModel
    {
        private readonly INewsArticleService _newsService;
        private readonly ICommentService _commentService;
        private readonly ICategoryService _categoryService;

        public NewsArticleViewModel News { get; set; } = new NewsArticleViewModel();
        public List<CommentViewModel> Comments { get; set; } = new List<CommentViewModel>();
        public List<NewsArticleViewModel> RelatedNews { get; set; } = new List<NewsArticleViewModel>();
        public List<NewsArticleViewModel> LatestNews { get; set; } = new List<NewsArticleViewModel>();
        public List<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();
        public bool IsLoggedIn { get; set; }

        public NewsDetailsModel(
            INewsArticleService newsService,
            ICommentService commentService,
            ICategoryService categoryService)
        {
            _newsService = newsService;
            _commentService = commentService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToPage("/Index");
            }

            var news = await _newsService.GetNewsByIdAsync(id);
            if (news == null || !news.NewsStatus) // Only show active news to public
            {
                return NotFound();
            }

            News = news;
            Comments = (await _commentService.GetCommentsByNewsArticleAsync(id)).ToList();
            RelatedNews = (await _newsService.GetRelatedNewsAsync(id, 5)).ToList();

            // Get latest news
            var allNews = await _newsService.GetActiveNewsAsync();
            LatestNews = allNews.Take(5).ToList();

            // Get categories
            Categories = (await _categoryService.GetActiveCategoriesAsync()).ToList();

            // Check if user is logged in
            IsLoggedIn = HttpContext.Session.GetInt32("UserId").HasValue;

            return Page();
        }
    }
}