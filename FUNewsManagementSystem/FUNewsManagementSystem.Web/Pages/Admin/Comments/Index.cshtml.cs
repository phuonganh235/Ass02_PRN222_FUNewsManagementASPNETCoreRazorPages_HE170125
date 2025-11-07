using BusinessLogic.Interfaces;
using BusinessLogic.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.Web.Pages.Admin.Comments
{
    public class IndexModel : PageModel
    {
        private readonly ICommentService _commentService;

        public List<CommentViewModel> Comments { get; set; } = new List<CommentViewModel>();

        public IndexModel(ICommentService commentService)
        {
            _commentService = commentService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            // Check if user is Admin
            var userRole = HttpContext.Session.GetInt32("UserRole");
            if (userRole != 0)
            {
                return RedirectToPage("/Index");
            }

            Comments = (await _commentService.GetAllCommentsAsync()).ToList();

            return Page();
        }

        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var comment = await _commentService.GetCommentByIdAsync(id);

            if (comment == null)
            {
                return new JsonResult(new { success = false, message = "Comment not found" });
            }

            // Store account ID before deleting to notify the user
            var accountId = comment.AccountId;
            var accountName = comment.AccountName;

            var result = await _commentService.DeleteCommentAsync(id);

            if (result)
            {
                return new JsonResult(new
                {
                    success = true,
                    message = "Comment deleted successfully",
                    accountId = accountId,
                    accountName = accountName
                });
            }

            return new JsonResult(new { success = false, message = "Failed to delete comment" });
        }
    }
}
