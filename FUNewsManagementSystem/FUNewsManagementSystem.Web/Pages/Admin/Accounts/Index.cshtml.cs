using BusinessLogic.Interfaces;
using BusinessLogic.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.Web.Pages.Admin.Accounts
{
    public class IndexModel : PageModel
    {
        private readonly ISystemAccountService _accountService;

        public List<AccountViewModel> Accounts { get; set; } = new List<AccountViewModel>();

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? Role { get; set; }

        public IndexModel(ISystemAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            // Check if user is Admin
            var userRole = HttpContext.Session.GetInt32("UserRole");
            if (userRole != 0)
            {
                return RedirectToPage("/Index");
            }

            if (!string.IsNullOrEmpty(SearchTerm) || Role.HasValue)
            {
                Accounts = (await _accountService.SearchAccountsAsync(SearchTerm, Role)).ToList();
            }
            else
            {
                Accounts = (await _accountService.GetAllAccountsAsync()).ToList();
            }

            return Page();
        }

        public async Task<IActionResult> OnGetGetAsync(short id)
        {
            var account = await _accountService.GetAccountByIdAsync(id);
            return new JsonResult(account);
        }

        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> OnPostCreateAsync([FromBody] AccountViewModel model)
        {
            // Validate password for new account
            if (string.IsNullOrWhiteSpace(model.AccountPassword))
            {
                return new JsonResult(new
                {
                    success = false,
                    message = "Password is required for new account"
                });
            }

            if (model.AccountPassword.Length < 6)
            {
                return new JsonResult(new
                {
                    success = false,
                    message = "Password must be at least 6 characters"
                });
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return new JsonResult(new
                {
                    success = false,
                    message = "Invalid data: " + string.Join(", ", errors),
                    errors = errors
                });
            }

            var result = await _accountService.CreateAccountAsync(model);

            if (result)
            {
                return new JsonResult(new { success = true, message = "Account created successfully" });
            }

            return new JsonResult(new { success = false, message = "Email already exists" });
        }

        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> OnPostUpdateAsync([FromBody] AccountViewModel model)
        {
            // Validate password if provided
            if (!string.IsNullOrWhiteSpace(model.AccountPassword) && model.AccountPassword.Length < 6)
            {
                return new JsonResult(new
                {
                    success = false,
                    message = "Password must be at least 6 characters"
                });
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return new JsonResult(new
                {
                    success = false,
                    message = "Invalid data: " + string.Join(", ", errors),
                    errors = errors
                });
            }

            var result = await _accountService.UpdateAccountAsync(model);

            if (result)
            {
                return new JsonResult(new { success = true, message = "Account updated successfully" });
            }

            return new JsonResult(new { success = false, message = "Email already exists or account not found" });
        }

        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> OnPostDeleteAsync(short id)
        {
            var canDelete = await _accountService.CanDeleteAccountAsync(id);

            if (!canDelete)
            {
                return new JsonResult(new
                {
                    success = false,
                    message = "Cannot delete this account because it has created news articles"
                });
            }

            var result = await _accountService.DeleteAccountAsync(id);

            if (result)
            {
                return new JsonResult(new { success = true, message = "Account deleted successfully" });
            }

            return new JsonResult(new { success = false, message = "Account not found" });
        }
    }
}
