using BusinessLogic.Interfaces;
using BusinessLogic.ViewModels;
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

        public async Task OnGetAsync()
        {
            Accounts = await _accountService.GetAllAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _accountService.DeleteAsync(id);
            return RedirectToPage();
        }

        public string GetRoleName(int roleId)
        {
            return roleId switch
            {
                3 => "Admin",
                2 => "Staff",
                1 => "Lecturer",
                _ => "Unknown"
            };
        }
    }
}
