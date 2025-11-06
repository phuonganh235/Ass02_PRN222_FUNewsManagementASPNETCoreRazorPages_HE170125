using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessLogic.Services;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace FUNewsManagementSystem.Web.Pages.Auth;

public class LoginModel : PageModel
{
    [BindProperty] public string Email { get; set; } = string.Empty;
    [BindProperty] public string Password { get; set; } = string.Empty;
    public string Error { get; set; } = "";

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        var opt = new DbContextOptionsBuilder<FUNewsDbContext>()
            .UseSqlServer("Server=.;Database=FUNewsManagement;Trusted_Connection=True;Encrypt=False")
            .Options;

        using var ctx = new FUNewsDbContext(opt);
        var auth = new AuthService(ctx);
        var user = await auth.LoginAsync(Email, Password);

        if (user == null)
        {
            Error = "Sai email hoặc mật khẩu!";
            return Page();
        }

        HttpContext.Session.SetString("UserId", user.AccountID.ToString());
        HttpContext.Session.SetString("UserRole", user.AccountRole.ToString());
        HttpContext.Session.SetString("UserName", user.AccountName);

        return RedirectToPage("/Dashboard/Index");
    }
}
