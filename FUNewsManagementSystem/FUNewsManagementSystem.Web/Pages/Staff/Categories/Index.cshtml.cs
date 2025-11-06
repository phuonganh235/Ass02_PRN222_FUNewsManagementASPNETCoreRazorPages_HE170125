using BusinessLogic.Interfaces;
using BusinessLogic.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.Web.Pages.Staff.Categories
{
    public class IndexModel : PageModel
    {
        private readonly ICategoryService _categoryService;

        public List<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();
        public List<CategoryViewModel> ParentCategories { get; set; } = new List<CategoryViewModel>();

        public IndexModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> OnGetAsync(string? searchTerm, bool? isActive)
        {
            // Check if user is Staff
            var userRole = HttpContext.Session.GetInt32("UserRole");
            if (userRole != 1)
            {
                return RedirectToPage("/Index");
            }

            // Get parent categories for dropdown
            ParentCategories = (await _categoryService.GetParentCategoriesAsync()).ToList();

            // Get categories based on filter
            if (!string.IsNullOrEmpty(searchTerm) || isActive.HasValue)
            {
                Categories = (await _categoryService.SearchCategoriesAsync(searchTerm, isActive)).ToList();
            }
            else
            {
                Categories = (await _categoryService.GetAllCategoriesAsync()).ToList();
            }

            return Page();
        }

        public async Task<IActionResult> OnGetGetAsync(short id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            return new JsonResult(category);
        }

        public async Task<IActionResult> OnPostCreateAsync([FromBody] CategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return new JsonResult(new { success = false, message = "Invalid data" });
            }

            var result = await _categoryService.CreateCategoryAsync(model);

            if (result)
            {
                return new JsonResult(new { success = true, message = "Category created successfully" });
            }

            return new JsonResult(new { success = false, message = "Category name already exists" });
        }

        public async Task<IActionResult> OnPostUpdateAsync([FromBody] CategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return new JsonResult(new { success = false, message = "Invalid data" });
            }

            var result = await _categoryService.UpdateCategoryAsync(model);

            if (result)
            {
                return new JsonResult(new { success = true, message = "Category updated successfully" });
            }

            return new JsonResult(new { success = false, message = "Category name already exists or category not found" });
        }

        public async Task<IActionResult> OnPostDeleteAsync(short id)
        {
            var canDelete = await _categoryService.CanDeleteCategoryAsync(id);

            if (!canDelete)
            {
                return new JsonResult(new
                {
                    success = false,
                    message = "Cannot delete this category because it has associated news articles"
                });
            }

            var result = await _categoryService.DeleteCategoryAsync(id);

            if (result)
            {
                return new JsonResult(new { success = true, message = "Category deleted successfully" });
            }

            return new JsonResult(new { success = false, message = "Category not found" });
        }
    }
}