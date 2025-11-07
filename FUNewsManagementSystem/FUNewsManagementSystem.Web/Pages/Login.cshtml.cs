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

            // ✅ BƯỚC 1: KIỂM TRA ADMIN TỪ APPSETTINGS TRƯỚC
            var adminEmail = _configuration["AdminAccount:Email"];
            var adminPassword = _configuration["AdminAccount:Password"];

            if (!string.IsNullOrEmpty(adminEmail) &&
                !string.IsNullOrEmpty(adminPassword) &&
                Input.Email.Equals(adminEmail, StringComparison.OrdinalIgnoreCase) &&
                Input.Password == adminPassword)
            {
                // Admin login từ appsettings
                HttpContext.Session.SetInt32("UserId", 0);
                HttpContext.Session.SetString("UserName", "Administrator");
                HttpContext.Session.SetString("UserEmail", adminEmail);
                HttpContext.Session.SetInt32("UserRole", 0);

                return RedirectToPage("/Admin/Dashboard");
            }

            // ✅ BƯỚC 2: SAU ĐÓ MỚI KIỂM TRA DATABASE
            var account = await _accountService.AuthenticateAsync(Input.Email, Input.Password);

            if (account == null)
            {
                ErrorMessage = "Invalid email or password.";
                return Page();
            }

            // Set session cho Staff/Lecturer từ database
            HttpContext.Session.SetInt32("UserId", account.AccountId);
            HttpContext.Session.SetString("UserName", account.AccountName);
            HttpContext.Session.SetString("UserEmail", account.AccountEmail);
            HttpContext.Session.SetInt32("UserRole", account.AccountRole);

            // Redirect theo role
            if (account.AccountRole == 0) // Admin từ DB
            {
                return RedirectToPage("/Admin/Dashboard");
            }
            else if (account.AccountRole == 1) // Staff
            {
                return RedirectToPage("/Staff/NewsArticles/Index");
            }
            else // Lecturer (role = 2)
            {
                return RedirectToPage("/Index");
            }
        }
    }
}