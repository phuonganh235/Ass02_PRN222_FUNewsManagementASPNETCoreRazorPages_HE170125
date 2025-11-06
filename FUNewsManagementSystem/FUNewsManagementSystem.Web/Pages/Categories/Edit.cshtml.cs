using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessLogic.Services;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using DataAccess.Entities;

namespace FUNewsManagementSystem.Web.Pages.Categories;

public class EditModel : PageModel
{
    [BindProperty] public Category? Item { get; set; }

    public async Task<IActionResult> OnGetAsync(short id)
    {
        var opt = new DbContextOptionsBuilder<FUNewsDbContext>()
            .UseSqlServer("Server=.;Database=FUNewsManagement;Trusted_Connection=True;Encrypt=False").Options;
        using var ctx = new FUNewsDbContext(opt);
        var svc = new CategoryService(ctx);

        Item = await svc.GetAsync(id);
        if (Item == null) return RedirectToPage("Index");
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (Item == null) return RedirectToPage("Index");
        var opt = new DbContextOptionsBuilder<FUNewsDbContext>()
            .UseSqlServer("Server=.;Database=FUNewsManagement;Trusted_Connection=True;Encrypt=False").Options;
        using var ctx = new FUNewsDbContext(opt);
        var svc = new CategoryService(ctx);

        await svc.UpdateAsync(Item);
        return RedirectToPage("Index");
    }
}
