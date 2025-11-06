using Microsoft.AspNetCore.Mvc.RazorPages;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace FUNewsManagementSystem.Web.Pages.Dashboard;

public class IndexModel : PageModel
{
    public int NewsCount { get; set; }
    public int CategoryCount { get; set; }
    public int AccountCount { get; set; }

    public async Task OnGet()
    {
        var opt = new DbContextOptionsBuilder<FUNewsDbContext>()
            .UseSqlServer("Server=.;Database=FUNewsManagement;Trusted_Connection=True;Encrypt=False")
            .Options;

        using var ctx = new FUNewsDbContext(opt);
        NewsCount = await ctx.NewsArticles.CountAsync();
        CategoryCount = await ctx.Categories.CountAsync();
        AccountCount = await ctx.SystemAccounts.CountAsync();
    }
}
