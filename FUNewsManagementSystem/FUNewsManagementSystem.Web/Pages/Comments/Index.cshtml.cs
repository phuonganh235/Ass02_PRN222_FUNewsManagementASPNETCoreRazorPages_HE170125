using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessLogic.Services;
using BusinessLogic.ViewModels;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace FUNewsManagementSystem.Web.Pages.Comments
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public string NewsArticleID { get; set; } = string.Empty;

        [BindProperty]
        public string Content { get; set; } = string.Empty;

        public List<CommentVM> CommentList { get; set; } = new();

        public async Task OnGetAsync(string id)
        {
            NewsArticleID = id;
            var opt = new DbContextOptionsBuilder<FUNewsDbContext>()
                .UseSqlServer("Server=.;Database=FUNewsManagement;Trusted_Connection=True;Encrypt=False")
                .Options;
            using var ctx = new FUNewsDbContext(opt);
            var commentService = new CommentService(ctx);
            CommentList = await commentService.GetByNewsAsync(id);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(Content)) return Page();

            var opt = new DbContextOptionsBuilder<FUNewsDbContext>()
                .UseSqlServer("Server=.;Database=FUNewsManagement;Trusted_Connection=True;Encrypt=False")
                .Options;
            using var ctx = new FUNewsDbContext(opt);
            var commentService = new CommentService(ctx);

            await commentService.AddAsync(NewsArticleID, accountId: 1, content: Content);

            return RedirectToPage(new { id = NewsArticleID });
        }
    }
}
