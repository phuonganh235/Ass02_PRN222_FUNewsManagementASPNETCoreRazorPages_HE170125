using BusinessLogic.Interfaces;
using BusinessLogic.ViewModels;
using FUNewsManagementSystem.Web.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;

namespace FUNewsManagementSystem.Web.Pages.Staff.NewsArticles
{
    public class EditModel : PageModel
    {
        private readonly INewsArticleService _newsService;
        private readonly ICategoryService _categoryService;
        private readonly ITagService _tagService;
        private readonly IHubContext<NotificationHub> _hubContext;

        [BindProperty]
        public NewsArticleViewModel Input { get; set; } = new NewsArticleViewModel();

        public List<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();
        public List<TagViewModel> AllTags { get; set; } = new List<TagViewModel>();

        public EditModel(
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

        public async Task<IActionResult> OnGetAsync(string id)
        {
            // Check if user is Staff
            var userRole = HttpContext.Session.GetInt32("UserRole");
            if (userRole != 1)
            {
                return RedirectToPage("/Index");
            }

            if (string.IsNullOrEmpty(id))
            {
                return RedirectToPage("Index");
            }

            var news = await _newsService.GetNewsByIdAsync(id);
            if (news == null)
            {
                return NotFound();
            }

            Input = news;
            Categories = (await _categoryService.GetActiveCategoriesAsync()).ToList();
            AllTags = (await _tagService.GetAllTagsAsync()).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Reload dropdowns
            Categories = (await _categoryService.GetActiveCategoriesAsync()).ToList();
            AllTags = (await _tagService.GetAllTagsAsync()).ToList();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                return RedirectToPage("/Login");
            }

            // Update news article
            var result = await _newsService.UpdateNewsAsync(Input, (short)userId.Value);

            if (result)
            {
                // Send SignalR notification
                await _hubContext.Clients.All.SendAsync(
                    "ReceiveNotification",
                    $"Article '{Input.NewsTitle}' has been updated",
                    "info"
                );

                await _hubContext.Clients.All.SendAsync("RefreshNewsList");

                TempData["SuccessMessage"] = "News article updated successfully!";
                return RedirectToPage("Index");
            }

            ModelState.AddModelError(string.Empty, "Failed to update news article");
            return Page();
        }
    }
}