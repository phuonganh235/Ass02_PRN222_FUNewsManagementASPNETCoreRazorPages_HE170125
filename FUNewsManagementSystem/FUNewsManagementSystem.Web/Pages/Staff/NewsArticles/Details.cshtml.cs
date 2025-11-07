using BusinessLogic.Interfaces;
using BusinessLogic.ViewModels;
using FUNewsManagementSystem.Web.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;

namespace FUNewsManagementSystem.Web.Pages.Staff.NewsArticles
{
    public class DetailsModel : PageModel
    {
        private readonly INewsArticleService _newsService;
        private readonly ICommentService _commentService;
        private readonly IHubContext<NotificationHub> _hubContext;

        public NewsArticleViewModel News { get; set; } = new NewsArticleViewModel();
        public List<CommentViewModel> Comments { get; set; } = new List<CommentViewModel>();
        public List<NewsArticleViewModel> RelatedNews { get; set; } = new List<NewsArticleViewModel>();

        public short? UserId { get; set; }
        public string? UserName { get; set; }
        public int? UserRole { get; set; }

        public DetailsModel(
            INewsArticleService newsService,
            ICommentService commentService,
            IHubContext<NotificationHub> hubContext)
        {
            _newsService = newsService;
            _commentService = commentService;
            _hubContext = hubContext;
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToPage("Index");
            }

            var news = await _newsService.GetNewsByIdAsync(id);
            if (news == null)
            {
                return NotFound();
            }

            News = news;
            Comments = (await _commentService.GetCommentsByNewsArticleAsync(id)).ToList();
            RelatedNews = (await _newsService.GetRelatedNewsAsync(id, 5)).ToList();

            // Get user info from session
            UserId = (short?)HttpContext.Session.GetInt32("UserId");
            UserName = HttpContext.Session.GetString("UserName");
            UserRole = HttpContext.Session.GetInt32("UserRole");

            return Page();
        }

        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> OnPostAddCommentAsync([FromBody] AddCommentRequest request)
        {
            Console.WriteLine($"[DEBUG] OnPostAddCommentAsync called");
            Console.WriteLine($"[DEBUG] NewsArticleId: {request?.NewsArticleId}, Content: '{request?.Content}'");

            var userId = HttpContext.Session.GetInt32("UserId");
            var userName = HttpContext.Session.GetString("UserName");

            Console.WriteLine($"[DEBUG] UserId: {userId}, UserName: {userName}");

            if (!userId.HasValue || string.IsNullOrEmpty(userName))
            {
                return new JsonResult(new { success = false, message = "You must be logged in to comment" });
            }

            if (string.IsNullOrWhiteSpace(request.Content))
            {
                return new JsonResult(new { success = false, message = "Comment content is required" });
            }

            try
            {
                var comment = await _commentService.CreateCommentAsync(
                    request.NewsArticleId,
                    (short)userId.Value,
                    request.Content
                );

                Console.WriteLine($"[DEBUG] Comment created: CommentId={comment.CommentId}");

                // Broadcast comment via SignalR with proper notification format
                await _hubContext.Clients.All.SendAsync(
                    "ReceiveComment",
                    request.NewsArticleId,
                    comment.CommentId.ToString(),
                    userName,
                    comment.Content,
                    comment.TimeAgo
                );

                // Dashboard update with detailed notification - matches NotificationHub format
                await _hubContext.Clients.Group("Group_Admin").SendAsync(
                    "DashboardUpdate",
                    "Comment",
                    "Created",
                    userName
                );

                // Refresh admin dashboard stats
                await _hubContext.Clients.Group("Group_Admin").SendAsync("RefreshDashboard");

                return new JsonResult(new { success = true, comment });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Exception in CreateComment: {ex.Message}");
                Console.WriteLine($"[ERROR] Stack trace: {ex.StackTrace}");
                return new JsonResult(new { success = false, message = ex.Message });
            }
        }

        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> OnPostDeleteCommentAsync(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var userRole = HttpContext.Session.GetInt32("UserRole");

            if (!userId.HasValue)
            {
                return new JsonResult(new { success = false, message = "Unauthorized" });
            }

            // Get comment to check ownership
            var comment = await _commentService.GetCommentByIdAsync(id);
            if (comment == null)
            {
                return new JsonResult(new { success = false, message = "Comment not found" });
            }

            // Only admin or comment owner can delete
            if (userRole != 0 && comment.AccountId != userId.Value)
            {
                return new JsonResult(new { success = false, message = "You don't have permission to delete this comment" });
            }

            var result = await _commentService.DeleteCommentAsync(id);

            if (result)
            {
                // Notify via SignalR
                await _hubContext.Clients.All.SendAsync("CommentDeleted", id, comment.NewsArticleId);

                // Dashboard update with detailed notification
                await _hubContext.Clients.Group("Group_Admin").SendAsync(
                    "DashboardUpdate",
                    "Comment",
                    "Deleted",
                    ""
                );

                // Refresh admin dashboard stats
                await _hubContext.Clients.Group("Group_Admin").SendAsync("RefreshDashboard");

                return new JsonResult(new { success = true, message = "Comment deleted successfully" });
            }

            return new JsonResult(new { success = false, message = "Failed to delete comment" });
        }

        public class AddCommentRequest
        {
            public string NewsArticleId { get; set; } = string.Empty;
            public string Content { get; set; } = string.Empty;
        }
    }
}