using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessLogic.Interfaces;


namespace FUNewsManagementSystem.Web.Pages.Staff.Categories
{
    [IgnoreAntiforgeryToken]
    public class GetModel : PageModel
    {
        private readonly ICategoryService _categoryService;


        public GetModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }


        public async Task<JsonResult> OnGetAsync(short id)
        {
            try
            {
                var category = await _categoryService.GetCategoryByIdAsync(id);
                if (category == null)
                    return new JsonResult(new { success = false, message = "Category not found." });


                return new JsonResult(category);
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = "Error: " + ex.Message });
            }
        }
    }
}