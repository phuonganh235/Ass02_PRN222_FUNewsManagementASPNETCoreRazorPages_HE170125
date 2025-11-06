using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessLogic.Services;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace FUNewsManagementSystem.Web.Pages.News
{
    public class Delete : PageModel
    {
        public async Task<IActionResult> OnGetAsync(string id)
        {
            var opt = new DbContextOptionsBuilder<FUNewsDbContext>()
                .UseSqlServer("Server=.;Database=FUNewsManagement;Trusted_Connection=True;Encrypt=False")
                .Options;
            using var ctx = new FUNewsDbContext(opt);

            var newsService = new NewsArticleService(ctx);
            await newsService.DeleteAsync(id);

            return RedirectToPage("Index");
        }
    }
}
