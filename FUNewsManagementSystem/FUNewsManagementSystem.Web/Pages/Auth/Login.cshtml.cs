using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.Web.Pages.Auth
{
    public class LoginModel : PageModel
    {
        private readonly IAuthService _authService;
        public LoginModel(IAuthService authService)
        {
            _authService = authService;
        }

        [BindProperty]
        public string Email { get; set; } = string.Empty;
        [BindProperty]
        public string Password { get; set; } = string.Empty;

        public string? ErrorMessage { get; set; }

        public void OnGet()
        {
            // clear session when go to login screen
            HttpContext.Session.Clear();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _authService.LoginAsync(Email, Password);

            if (user == null)
            {
                ErrorMessage = "Invalid email or password.";
                return Page();
            }

            // store session
            HttpContext.Session.SetInt32("AccountID", user.AccountID);
            HttpContext.Session.SetString("AccountName", user.AccountName ?? "");
            HttpContext.Session.SetString("AccountEmail", user.AccountEmail ?? "");
            HttpContext.Session.SetInt32("AccountRole", user.AccountRole);

            return RedirectToPage("/Dashboard/Index");
        }
    }
}
