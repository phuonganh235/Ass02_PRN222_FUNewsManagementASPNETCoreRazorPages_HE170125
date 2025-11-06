using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessLogic.Services;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using DataAccess.Entities;

namespace FUNewsManagementSystem.Web.Pages.Categories;

public class IndexModel : PageModel
{
    public List<Category> Items { get; set; } = new();

    public async Task OnGet()
    {
        var opt = new DbContextOptionsBuilder<FUNewsDbContext>()
            .UseSqlServer("Server=.;Database=FUNewsManagement;Trusted_Connection=True;Encrypt=False").Options;
        using var ctx = new FUNewsDbContext(opt);
        var svc = new CategoryService(ctx);
        Items = await svc.GetAllAsync();
    }
}
