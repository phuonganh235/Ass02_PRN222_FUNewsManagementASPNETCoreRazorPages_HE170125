using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessLogic.Services;
using DataAccess.Context;
using BusinessLogic.ViewModels;
using Microsoft.EntityFrameworkCore;
using DataAccess.Entities;

namespace FUNewsManagementSystem.Web.Pages.News
{
    public class DetailModel : PageModel
    {
        public NewsArticle? Article { get; set; }
        public List<CommentVM> Comments { get; set; } = new();

        public async Task OnGetAsync(string id)
        {
            var options = new DbContextOptionsBuilder<FUNewsDbContext>()
                .UseSqlServer("Server=.;Database=FUNewsManagement;Trusted_Connection=True;Encrypt=False")
                .Options;

            using var ctx = new FUNewsDbContext(options);
            var newsService = new NewsArticleService(ctx);
            var commentService = new CommentService(ctx);

            Article = await newsService.GetAsync(id);
            Comments = await commentService.GetByNewsAsync(id);
        }
    }
}
