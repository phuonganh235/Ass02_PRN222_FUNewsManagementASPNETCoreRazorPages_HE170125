using BusinessLogic.Interfaces;
using BusinessLogic.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;

namespace FUNewsManagementSystem.Web.Pages.Comments
{
    public class IndexModel : PageModel
    {
        private readonly ICommentService _commentService;

        public IndexModel(ICommentService commentService)
        {
            _commentService = commentService;
        }

        public List<CommentVM> CommentList { get; set; } = new();

        public async Task OnGetAsync()
        {
            CommentList = await _commentService.GetAllAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int commentId)
        {
            int? role = HttpContext.Session.GetInt32("AccountRole");

            if (role != 3)
            {
                TempData["ErrorMessage"] = "You are not authorized to delete comments.";
                return RedirectToPage();
            }

            bool result = await _commentService.DeleteAsync(commentId);

            if (result)
                TempData["SuccessMessage"] = "Comment deleted successfully!";
            else
                TempData["ErrorMessage"] = "Failed to delete comment.";

            return RedirectToPage();
        }
    }
}
