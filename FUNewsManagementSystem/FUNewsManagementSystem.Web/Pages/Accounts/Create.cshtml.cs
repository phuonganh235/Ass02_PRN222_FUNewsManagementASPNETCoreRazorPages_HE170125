using BusinessLogic.Interfaces;
using BusinessLogic.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.Web.Pages.Accounts
{
    public class CreateModel : PageModel
    {
        private readonly IAccountService _accountService;

        public CreateModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [BindProperty]
        public AccountVM Account { get; set; } = new();

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            await _accountService.CreateAsync(Account);
            return RedirectToPage("Index");
        }
    }
}
