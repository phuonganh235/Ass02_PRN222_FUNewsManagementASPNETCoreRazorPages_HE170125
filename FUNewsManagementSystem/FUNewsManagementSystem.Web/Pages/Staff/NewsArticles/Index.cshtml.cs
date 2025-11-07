using BusinessLogic.Interfaces;
using BusinessLogic.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.Web.Pages.Staff.NewsArticles
{
    public class IndexModel : PageModel
    {
        private readonly INewsArticleService _newsService;
        private readonly ICategoryService _categoryService;

        public List<NewsArticleViewModel> NewsList { get; set; } = new List<NewsArticleViewModel>();
        public List<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public short? CategoryId { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool? Status { get; set; }

        public IndexModel(INewsArticleService newsService, ICategoryService categoryService)
        {
            _newsService = newsService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            // Check if user is Staff
            var userRole = HttpContext.Session.GetInt32("UserRole");
            if (userRole != 1)
            {
                return RedirectToPage("/Index");
            }

            // Get categories for filter
            Categories = (await _categoryService.GetActiveCategoriesAsync()).ToList();

            // Get news articles with filters
            if (!string.IsNullOrEmpty(SearchTerm) || CategoryId.HasValue || Status.HasValue)
            {
                NewsList = (await _newsService.SearchNewsAsync(
                    SearchTerm,
                    CategoryId,
                    Status,
                    null,
                    null)).ToList();
            }
            else
            {
                NewsList = (await _newsService.GetAllNewsAsync()).ToList();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            var result = await _newsService.DeleteNewsAsync(id);

            if (result)
            {
                return new JsonResult(new { success = true, message = "News article deleted successfully" });
            }

            return new JsonResult(new { success = false, message = "News article not found" });
        }

        public async Task<IActionResult> OnPostDuplicateAsync(string id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return new JsonResult(new { success = false, message = "User not authenticated" });
            }

            var newId = await _newsService.DuplicateNewsAsync(id, (short)userId.Value);

            if (newId != null)
            {
                return new JsonResult(new
                {
                    success = true,
                    message = "Article duplicated successfully",
                    newId = newId
                });
            }

            return new JsonResult(new { success = false, message = "Failed to duplicate article" });
        }
    }
}