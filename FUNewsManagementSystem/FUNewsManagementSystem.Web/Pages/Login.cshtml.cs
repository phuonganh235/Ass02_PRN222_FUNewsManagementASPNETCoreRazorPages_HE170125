using BusinessLogic.Interfaces;
using BusinessLogic.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.Web.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ISystemAccountService _accountService;
        private readonly IConfiguration _configuration;

        [BindProperty]
        public LoginViewModel Input { get; set; } = new LoginViewModel();

        public string? ErrorMessage { get; set; }

        public LoginModel(ISystemAccountService accountService, IConfiguration configuration)
        {
            _accountService = accountService;
            _configuration = configuration;
        }

        public void OnGet()
        {
            // Clear any existing session
            HttpContext.Session.Clear();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var account = await _accountService.AuthenticateAsync(Input.Email, Input.Password);

            if (account == null)
            {
                ErrorMessage = "Invalid email or password.";
                return Page();
            }

            // Set session
            HttpContext.Session.SetInt32("UserId", account.AccountId);
            HttpContext.Session.SetString("UserName", account.AccountName);
            HttpContext.Session.SetString("UserEmail", account.AccountEmail);
            HttpContext.Session.SetInt32("UserRole", account.AccountRole);

            // Redirect based on role
            if (account.AccountRole == 0) // Admin
            {
                return RedirectToPage("/Admin/Dashboard");
            }
            else if (account.AccountRole == 1) // Staff
            {
                return RedirectToPage("/Staff/NewsArticles/Index");
            }
            else // Lecturer
            {
                return RedirectToPage("/Index");
            }
        }
    }
}