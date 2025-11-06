using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessLogic.Services;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using DataAccess.Entities;

namespace FUNewsManagementSystem.Web.Pages.Categories;

public class Create : PageModel
{
    [BindProperty] public Category Item { get; set; } = new() { IsActive = true };

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        var opt = new DbContextOptionsBuilder<FUNewsDbContext>()
            .UseSqlServer("Server=.;Database=FUNewsManagement;Trusted_Connection=True;Encrypt=False").Options;
        using var ctx = new FUNewsDbContext(opt);
        var svc = new CategoryService(ctx);

        await svc.AddAsync(Item);
        return RedirectToPage("Index");
    }
}
