using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessLogic.Services;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using DataAccess.Entities;

namespace FUNewsManagementSystem.Web.Pages.News
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public NewsArticle Input { get; set; } = new();

        public List<Category> Categories { get; set; } = new();

        public async Task OnGetAsync()
        {
            var opt = new DbContextOptionsBuilder<FUNewsDbContext>()
                .UseSqlServer("Server=.;Database=FUNewsManagement;Trusted_Connection=True;Encrypt=False")
                .Options;
            using var ctx = new FUNewsDbContext(opt);
            var catService = new CategoryService(ctx);
            Categories = await catService.GetAllAsync(true);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var opt = new DbContextOptionsBuilder<FUNewsDbContext>()
                .UseSqlServer("Server=.;Database=FUNewsManagement;Trusted_Connection=True;Encrypt=False")
                .Options;
            using var ctx = new FUNewsDbContext(opt);
            var newsService = new NewsArticleService(ctx);

            // tạo ID ngẫu nhiên
            string id = Guid.NewGuid().ToString("N").Substring(0, 10);

            await newsService.CreateAsync(
                id,
                Input.NewsTitle,
                Input.Headline,
                Input.NewsContent,
                Input.NewsSource,
                Input.CategoryID,
                Input.NewsStatus,
                createdById: 1,
                tagIds: null);

            return RedirectToPage("Index");
        }
    }
}
