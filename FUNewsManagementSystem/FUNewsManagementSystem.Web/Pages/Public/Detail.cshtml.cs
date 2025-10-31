using BusinessLogic.Interfaces;
using BusinessLogic.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;

namespace FUNewsManagementSystem.Web.Pages.Public
{
    public class DetailModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly ICommentService _commentService;

        public DetailModel(INewsArticleService newsArticleService, ICommentService commentService)
        {
            _newsArticleService = newsArticleService;
            _commentService = commentService;
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public NewsArticleVM NewsDetail { get; set; } = new();
        public List<CommentVM> CommentList { get; set; } = new();
        public string CurrentUserName { get; set; } = "Guest";

        public async Task<IActionResult> OnGetAsync()
        {
            var news = await _newsArticleService.GetByIdAsync(Id);
            if (news == null)
            {
                return RedirectToPage("/Public/Index");
            }

            NewsDetail = news;
            CommentList = await _commentService.GetByNewsAsync(Id);

            // Lấy tên người dùng từ session nếu có
            CurrentUserName = HttpContext.Session.GetString("AccountName") ?? "Guest";

            return Page();
        }

        public async Task<IActionResult> OnPostAddCommentAsync(int newsArticleId, string commentText, string commentBy)
        {
            if (string.IsNullOrWhiteSpace(commentText))
            {
                TempData["ErrorMessage"] = "Comment cannot be empty.";
                return RedirectToPage(new { id = newsArticleId });
            }

            bool result = await _commentService.AddAsync(newsArticleId, commentText, commentBy);

            if (result)
                TempData["SuccessMessage"] = "Your comment has been added!";
            else
                TempData["ErrorMessage"] = "Failed to add comment. Please try again.";

            return RedirectToPage(new { id = newsArticleId });
        }
    }
}
