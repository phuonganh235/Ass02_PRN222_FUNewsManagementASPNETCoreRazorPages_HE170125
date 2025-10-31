using BusinessLogic.Interfaces;
using BusinessLogic.ViewModels;
using FUNewsManagementSystem.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.Web.Pages.News
{
    public class IndexModel : PageModel
    {
        private readonly INewsArticleService _newsService;
        private readonly ICategoryService _catService;
        private readonly ITagService _tagService;

        public IndexModel(
            INewsArticleService newsService,
            ICategoryService categoryService,
            ITagService tagService)
        {
            _newsService = newsService;
            _catService = categoryService;
            _tagService = tagService;
        }

        public List<NewsArticleVM> NewsList { get; set; } = new();
        public List<CategoryVM> AllCategories { get; set; } = new();
        public List<TagVM> AllTags { get; set; } = new();

        [BindProperty]
        public int NewsArticleID { get; set; }

        [BindProperty]
        public string? NewsTitle { get; set; }
        [BindProperty]
        public string? NewsContent { get; set; }
        [BindProperty]
        public string? NewsSource { get; set; }

        [BindProperty]
        public int CategoryID { get; set; }
        [BindProperty]
        public int NewsStatus { get; set; } // 0 draft /1 published/2 archived

        [BindProperty]
        public List<int> SelectedTagIDs { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var guard = AuthGuard.RequireLogin(this);
            if (guard != null) return guard;
            // Admin & Staff có quyền News
            if (!(AuthGuard.IsAdmin(this) || AuthGuard.IsStaff(this)))
            {
                return RedirectToPage("/Dashboard/Index");
            }

            NewsList = await _newsService.GetAllAsync();
            AllCategories = await _catService.GetAllAsync();
            AllTags = await _tagService.GetAllAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            if (!(AuthGuard.IsAdmin(this) || AuthGuard.IsStaff(this)))
            {
                return RedirectToPage("/Dashboard/Index");
            }

            var userId = HttpContext.Session.GetInt32("AccountID") ?? 0;

            await _newsService.CreateAsync(
                NewsTitle ?? "",
                NewsContent ?? "",
                NewsSource ?? "",
                CategoryID,
                userId,
                NewsStatus,
                SelectedTagIDs
            );

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUpdateAsync()
        {
            if (!(AuthGuard.IsAdmin(this) || AuthGuard.IsStaff(this)))
            {
                return RedirectToPage("/Dashboard/Index");
            }

            var userId = HttpContext.Session.GetInt32("AccountID") ?? 0;

            await _newsService.UpdateAsync(
                NewsArticleID,
                NewsTitle ?? "",
                NewsContent ?? "",
                NewsSource ?? "",
                CategoryID,
                userId,
                NewsStatus,
                SelectedTagIDs
            );

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            if (!(AuthGuard.IsAdmin(this) || AuthGuard.IsStaff(this)))
            {
                return RedirectToPage("/Dashboard/Index");
            }

            await _newsService.DeleteAsync(id);
            return RedirectToPage();
        }
    }
}
