using BusinessLogic.Interfaces;
using BusinessLogic.ViewModels;
using FUNewsManagementSystem.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.Web.Pages.Tags
{
    public class IndexModel : PageModel
    {
        private readonly ITagService _tagService;
        public IndexModel(ITagService tagService)
        {
            _tagService = tagService;
        }

        public List<TagVM> Tags { get; set; } = new();

        [BindProperty]
        public TagVM Input { get; set; } = new();

        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var guard = AuthGuard.RequireLogin(this);
            if (guard != null) return guard;
            if (!AuthGuard.IsAdmin(this)) return RedirectToPage("/Dashboard/Index");

            Tags = await _tagService.GetAllAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            if (!AuthGuard.IsAdmin(this)) return RedirectToPage("/Dashboard/Index");

            var ok = await _tagService.CreateAsync(Input);
            if (!ok) ErrorMessage = "Duplicate TagName.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUpdateAsync()
        {
            if (!AuthGuard.IsAdmin(this)) return RedirectToPage("/Dashboard/Index");

            var ok = await _tagService.UpdateAsync(Input);
            if (!ok) ErrorMessage = "Update failed (duplicate?).";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int tagId)
        {
            if (!AuthGuard.IsAdmin(this)) return RedirectToPage("/Dashboard/Index");

            await _tagService.DeleteAsync(tagId);
            return RedirectToPage();
        }
    }
}
