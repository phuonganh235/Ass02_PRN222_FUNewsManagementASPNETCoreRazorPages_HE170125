using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.Web.Pages
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            // Clear all session data
            HttpContext.Session.Clear();

            // Redirect to login page
            return RedirectToPage("/Login");
        }
    }
}