using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessLogic.Services;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using DataAccess.Entities;

namespace FUNewsManagementSystem.Web.Pages.News
{
    public class Edit : PageModel
    {
        [BindProperty]
        public NewsArticle Input { get; set; } = new();

        public List<Category> Categories { get; set; } = new();

        public async Task OnGetAsync(string id)
        {
            var opt = new DbContextOptionsBuilder<FUNewsDbContext>()
                .UseSqlServer("Server=.;Database=FUNewsManagement;Trusted_Connection=True;Encrypt=False")
                .Options;
            using var ctx = new FUNewsDbContext(opt);

            var catService = new CategoryService(ctx);
            Categories = await catService.GetAllAsync(true);

            var newsService = new NewsArticleService(ctx);
            Input = await newsService.GetAsync(id) ?? new NewsArticle();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            var opt = new DbContextOptionsBuilder<FUNewsDbContext>()
                .UseSqlServer("Server=.;Database=FUNewsManagement;Trusted_Connection=True;Encrypt=False")
                .Options;
            using var ctx = new FUNewsDbContext(opt);

            var newsService = new NewsArticleService(ctx);
            await newsService.UpdateAsync(
                id,
                Input.NewsTitle,
                Input.Headline,
                Input.NewsContent,
                Input.NewsSource,
                Input.CategoryID,
                Input.NewsStatus,
                updatedById: 1,
                tagIds: null);

            return RedirectToPage("Index");
        }
    }
}
