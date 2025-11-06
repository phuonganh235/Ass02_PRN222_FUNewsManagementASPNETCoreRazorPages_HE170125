using BusinessLogic.Interfaces;
using BusinessLogic.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.Web.Pages
{
    public class ProfileModel : PageModel
    {
        private readonly ISystemAccountService _accountService;
        private readonly INewsArticleService _newsService;

        [BindProperty]
        public AccountViewModel Input { get; set; } = new AccountViewModel();

        public int? UserRole { get; set; }
        public int ArticleCount { get; set; }
        public string MemberSince { get; set; } = string.Empty;

        public ProfileModel(ISystemAccountService accountService, INewsArticleService newsService)
        {
            _accountService = accountService;
            _newsService = newsService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return RedirectToPage("/Login");
            }

            var account = await _accountService.GetAccountByIdAsync((short)userId.Value);
            if (account == null)
            {
                return RedirectToPage("/Login");
            }

            Input = account;
            UserRole = account.AccountRole;

            // Get article count for Staff
            if (UserRole == 1)
            {
                var articles = await _newsService.GetNewsByAuthorAsync((short)userId.Value);
                ArticleCount = articles.Count();
            }

            // Simulate member since date (you can add this field to database)
            MemberSince = DateTime.Now.AddMonths(-6).ToString("MMMM yyyy");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue || Input.AccountId != userId.Value)
            {
                return RedirectToPage("/Login");
            }

            // Don't allow password change through this form
            Input.AccountPassword = null;

            var result = await _accountService.UpdateAccountAsync(Input);

            if (result)
            {
                // Update session
                HttpContext.Session.SetString("UserName", Input.AccountName);
                HttpContext.Session.SetString("UserEmail", Input.AccountEmail);

                TempData["SuccessMessage"] = "Profile updated successfully!";
                return RedirectToPage();
            }

            TempData["ErrorMessage"] = "Failed to update profile. Email may already be in use.";
            return Page();
        }

        public async Task<IActionResult> OnPostChangePasswordAsync(
            string currentPassword,
            string newPassword,
            string confirmPassword)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var userEmail = HttpContext.Session.GetString("UserEmail");

            if (!userId.HasValue || string.IsNullOrEmpty(userEmail))
            {
                return RedirectToPage("/Login");
            }

            if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 6)
            {
                TempData["ErrorMessage"] = "New password must be at least 6 characters long.";
                return RedirectToPage();
            }

            if (newPassword != confirmPassword)
            {
                TempData["ErrorMessage"] = "New password and confirmation do not match.";
                return RedirectToPage();
            }

            // Verify current password
            var authenticated = await _accountService.AuthenticateAsync(userEmail, currentPassword);
            if (authenticated == null)
            {
                TempData["ErrorMessage"] = "Current password is incorrect.";
                return RedirectToPage();
            }

            // Update password
            var account = await _accountService.GetAccountByIdAsync((short)userId.Value);
            if (account == null)
            {
                return RedirectToPage("/Login");
            }

            account.AccountPassword = newPassword;
            var result = await _accountService.UpdateAccountAsync(account);

            if (result)
            {
                TempData["SuccessMessage"] = "Password changed successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to change password.";
            }

            return RedirectToPage();
        }
    }
}