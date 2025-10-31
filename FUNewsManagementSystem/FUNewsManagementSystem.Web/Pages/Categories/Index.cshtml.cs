using BusinessLogic.Interfaces;
using BusinessLogic.ViewModels;
using FUNewsManagementSystem.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.Web.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly ICategoryService _catService;
        public IndexModel(ICategoryService catService)
        {
            _catService = catService;
        }

        public List<CategoryVM> Categories { get; set; } = new();

        [BindProperty]
        public CategoryVM Input { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var guard = AuthGuard.RequireLogin(this);
            if (guard != null) return guard;
            if (!AuthGuard.IsAdmin(this)) return RedirectToPage("/Dashboard/Index");

            Categories = await _catService.GetAllAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            if (!AuthGuard.IsAdmin(this)) return RedirectToPage("/Dashboard/Index");

            await _catService.CreateAsync(Input);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUpdateAsync()
        {
            if (!AuthGuard.IsAdmin(this)) return RedirectToPage("/Dashboard/Index");

            await _catService.UpdateAsync(Input);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int categoryId)
        {
            if (!AuthGuard.IsAdmin(this)) return RedirectToPage("/Dashboard/Index");

            await _catService.DeleteAsync(categoryId);
            return RedirectToPage();
        }
    }
}
