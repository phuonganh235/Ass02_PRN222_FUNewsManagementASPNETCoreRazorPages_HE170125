using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using BusinessLogic.Interfaces;
using BusinessLogic.ViewModels;


namespace FUNewsManagementSystem.Web.Pages.Staff.Categories
{
    [IgnoreAntiforgeryToken]
    public class UpdateModel : PageModel
    {
        private readonly ICategoryService _categoryService;


        public UpdateModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }


        public async Task<JsonResult> OnPostAsync()
        {
            try
            {
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();
                var model = JsonSerializer.Deserialize<CategoryViewModel>(body, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });


                if (model == null || model.CategoryId <= 0)
                    return new JsonResult(new { success = false, message = "Invalid category data." });


                var success = await _categoryService.UpdateCategoryAsync(model);


                if (!success)
                    return new JsonResult(new { success = false, message = "Update failed. Category name may already exist." });


                return new JsonResult(new { success = true, message = "Category updated successfully." });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = "Error: " + ex.Message });
            }
        }
    }
}