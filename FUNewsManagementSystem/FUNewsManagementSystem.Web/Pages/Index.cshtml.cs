using BusinessLogic.Interfaces;
using BusinessLogic.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly INewsArticleService _newsService;
        private readonly ICategoryService _categoryService;

        public List<NewsArticleViewModel> NewsList { get; set; } = new List<NewsArticleViewModel>();
        public List<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();

        public IndexModel(INewsArticleService newsService, ICategoryService categoryService)
        {
            _newsService = newsService;
            _categoryService = categoryService;
        }

        public async Task OnGetAsync(string? searchTerm, short? categoryId)
        {
            // Get active categories for filter dropdown
            Categories = (await _categoryService.GetActiveCategoriesAsync()).ToList();

            // Get news articles
            if (!string.IsNullOrEmpty(searchTerm) || categoryId.HasValue)
            {
                NewsList = (await _newsService.SearchNewsAsync(
                    searchTerm,
                    categoryId,
                    true, // Only active news
                    null,
                    null)).ToList();
            }
            else
            {
                NewsList = (await _newsService.GetActiveNewsAsync()).ToList();
            }
        }
    }
}