using BusinessLogic.Interfaces;
using BusinessLogic.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.Web.Pages.Accounts
{
    public class EditModel : PageModel
    {
        private readonly IAccountService _accountService;

        public EditModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [BindProperty]
        public AccountVM Account { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var account = await _accountService.GetByIdAsync(id);
            if (account == null)
                return RedirectToPage("Index");

            Account = account;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            await _accountService.UpdateAsync(Account);
            return RedirectToPage("Index");
        }
    }
}
