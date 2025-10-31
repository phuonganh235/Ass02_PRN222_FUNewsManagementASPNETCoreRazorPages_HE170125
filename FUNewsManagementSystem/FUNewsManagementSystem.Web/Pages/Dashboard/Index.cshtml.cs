using FUNewsManagementSystem.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.Web.Pages.Dashboard
{
    public class IndexModel : PageModel
    {
        public string? WelcomeName { get; set; }
        public string RoleName { get; set; } = "";

        public IActionResult OnGet()
        {
            var guard = AuthGuard.RequireLogin(this);
            if (guard != null) return guard;

            WelcomeName = HttpContext.Session.GetString("AccountName");
            var role = HttpContext.Session.GetInt32("AccountRole");

            RoleName = role switch
            {
                3 => "Admin",
                2 => "Staff",
                1 => "Lecturer",
                _ => "Unknown"
            };

            return Page();
        }
    }
}
