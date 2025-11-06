using BusinessLogic.Interfaces;
using BusinessLogic.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.Web.Pages.Staff.Tags
{
    public class IndexModel : PageModel
    {
        private readonly ITagService _tagService;

        public List<TagViewModel> Tags { get; set; } = new List<TagViewModel>();

        public IndexModel(ITagService tagService)
        {
            _tagService = tagService;
        }

        public async Task<IActionResult> OnGetAsync(string? searchTerm)
        {
            // Check if user is Staff
            var userRole = HttpContext.Session.GetInt32("UserRole");
            if (userRole != 1)
            {
                return RedirectToPage("/Index");
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                Tags = (await _tagService.SearchTagsAsync(searchTerm)).ToList();
            }
            else
            {
                Tags = (await _tagService.GetAllTagsAsync()).ToList();
            }

            return Page();
        }

        public async Task<IActionResult> OnGetGetAsync(int id)
        {
            var tag = await _tagService.GetTagByIdAsync(id);
            return new JsonResult(tag);
        }

        public async Task<IActionResult> OnPostCreateAsync([FromBody] TagViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return new JsonResult(new { success = false, message = "Invalid data" });
            }

            var result = await _tagService.CreateTagAsync(model);

            if (result)
            {
                return new JsonResult(new { success = true, message = "Tag created successfully" });
            }

            return new JsonResult(new { success = false, message = "Tag name already exists" });
        }

        public async Task<IActionResult> OnPostUpdateAsync([FromBody] TagViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return new JsonResult(new { success = false, message = "Invalid data" });
            }

            var result = await _tagService.UpdateTagAsync(model);

            if (result)
            {
                return new JsonResult(new { success = true, message = "Tag updated successfully" });
            }

            return new JsonResult(new { success = false, message = "Tag name already exists or tag not found" });
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var result = await _tagService.DeleteTagAsync(id);

            if (result)
            {
                return new JsonResult(new { success = true, message = "Tag deleted successfully" });
            }

            return new JsonResult(new { success = false, message = "Tag not found" });
        }
    }
}