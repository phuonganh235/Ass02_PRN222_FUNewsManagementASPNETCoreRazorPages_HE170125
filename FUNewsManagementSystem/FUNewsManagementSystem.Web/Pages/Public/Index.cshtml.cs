using BusinessLogic.Interfaces;
using BusinessLogic.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.Web.Pages.Public
{
    public class IndexModel : PageModel
    {
        private readonly INewsArticleService _newsService;
        public IndexModel(INewsArticleService newsService)
        {
            _newsService = newsService;
        }

        public List<NewsArticleVM> PublishedNews { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var all = await _newsService.GetAllAsync();
            // chỉ lấy bài đã publish (NewsStatus == 1)
            PublishedNews = all
                .Where(n => n.NewsStatus == 1)
                .OrderByDescending(n => n.NewsArticleID)
                .ToList();

            return Page();
        }
    }
}
