using BusinessLogic.Interfaces;
using BusinessLogic.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.Web.Pages.Public
{
    public class IndexModel : PageModel
    {
        private readonly INewsArticleService _newsSrv;

        public IndexModel(INewsArticleService newsSrv)
        {
            _newsSrv = newsSrv;
        }

        // View đang dùng PublishedNews -> giữ đúng tên này
        public List<NewsArticleVM> PublishedNews { get; set; } = new();

        public async Task OnGetAsync()
        {
            var all = await _newsSrv.GetAllAsync();

            // chỉ show bài có NewsStatus = true (public)
            PublishedNews = all
                .Where(n => n.NewsStatus)
                .OrderByDescending(n => n.CreatedDate)
                .ToList();
        }
    }
}
