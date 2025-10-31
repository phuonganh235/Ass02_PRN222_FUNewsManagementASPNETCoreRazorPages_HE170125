using BusinessLogic.Interfaces;
using BusinessLogic.ViewModels;
using FUNewsManagementSystem.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.Web.Pages.Accounts
{
    public class IndexModel : PageModel
    {
        private readonly IAccountService _accountService;
        public IndexModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public List<AccountVM> Accounts { get; set; } = new();

        [BindProperty]
        public AccountVM Input { get; set; } = new();

        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var guard = AuthGuard.RequireLogin(this);
            if (guard != null) return guard;
            if (!AuthGuard.IsAdmin(this)) return RedirectToPage("/Dashboard/Index");

            Accounts = await _accountService.GetAllAsync();
            return Page();
        }

        // CREATE
        public async Task<IActionResult> OnPostCreateAsync()
        {
            if (!AuthGuard.IsAdmin(this)) return RedirectToPage("/Dashboard/Index");

            var ok = await _accountService.CreateAsync(Input);
            if (!ok)
            {
                ErrorMessage = "Create failed.";
            }
            return RedirectToPage();
        }

        // UPDATE
        public async Task<IActionResult> OnPostUpdateAsync()
        {
            if (!AuthGuard.IsAdmin(this)) return RedirectToPage("/Dashboard/Index");

            var ok = await _accountService.UpdateAsync(Input);
            if (!ok)
            {
                ErrorMessage = "Update failed.";
            }
            return RedirectToPage();
        }

        // DELETE
        public async Task<IActionResult> OnPostDeleteAsync(int accountId)
        {
            if (!AuthGuard.IsAdmin(this)) return RedirectToPage("/Dashboard/Index");

            await _accountService.DeleteAsync(accountId);
            return RedirectToPage();
        }
    }
}
