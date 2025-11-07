using BusinessLogic.Interfaces;
using BusinessLogic.ViewModels;
using FUNewsManagementSystem.Web.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;

namespace FUNewsManagementSystem.Web.Pages.Staff.NewsArticles
{
    public class CreateModel : PageModel
    {
        private readonly INewsArticleService _newsService;
        private readonly ICategoryService _categoryService;
        private readonly ITagService _tagService;
        private readonly IHubContext<NotificationHub> _hubContext;

        [BindProperty]
        public NewsArticleViewModel Input { get; set; } = new NewsArticleViewModel();

        public List<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();
        public List<TagViewModel> AllTags { get; set; } = new List<TagViewModel>();

        public CreateModel(
            INewsArticleService newsService,
            ICategoryService categoryService,
            ITagService tagService,
            IHubContext<NotificationHub> hubContext)
        {
            _newsService = newsService;
            _categoryService = categoryService;
            _tagService = tagService;
            _hubContext = hubContext;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            // Check if user is Staff
            var userRole = HttpContext.Session.GetInt32("UserRole");
            if (userRole != 1)
            {
                return RedirectToPage("/Index");
            }

            Categories = (await _categoryService.GetActiveCategoriesAsync()).ToList();
            AllTags = (await _tagService.GetAllTagsAsync()).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Reload dropdowns in case of validation error
            Categories = (await _categoryService.GetActiveCategoriesAsync()).ToList();
            AllTags = (await _tagService.GetAllTagsAsync()).ToList();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Check if ID already exists
            if (await _newsService.NewsIdExistsAsync(Input.NewsArticleId))
            {
                ModelState.AddModelError("Input.NewsArticleId", "This News Article ID already exists");
                return Page();
            }

            var userId = HttpContext.Session.GetInt32("UserId");
            var userName = HttpContext.Session.GetString("UserName");

            if (!userId.HasValue)
            {
                return RedirectToPage("/Login");
            }

            // Create news article
            var result = await _newsService.CreateNewsAsync(Input, (short)userId.Value);

            if (result)
            {
                // Send SignalR notifications via NotificationHub methods
                // This will automatically send "DashboardUpdate" to Admin
                await _hubContext.Clients.Group("Group_Staff").SendAsync(
                    "NewsArticleCreated",
                    Input.NewsTitle,
                    userName
                );

                // Dashboard update with detailed notification
                await _hubContext.Clients.Group("Group_Admin").SendAsync(
                    "DashboardUpdate",
                    "Article",
                    "Created",
                    Input.NewsTitle
                );

                // Refresh news list on all pages
                await _hubContext.Clients.All.SendAsync("RefreshNewsList");

                // Refresh admin dashboard
                await _hubContext.Clients.Group("Group_Admin").SendAsync("RefreshDashboard");

                TempData["SuccessMessage"] = "News article created successfully!";
                return RedirectToPage("Index");
            }

            ModelState.AddModelError(string.Empty, "Failed to create news article");
            return Page();
        }
    }
}