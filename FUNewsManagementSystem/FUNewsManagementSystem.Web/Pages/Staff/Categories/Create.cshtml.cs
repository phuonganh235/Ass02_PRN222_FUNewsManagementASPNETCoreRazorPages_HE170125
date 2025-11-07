using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using BusinessLogic.Interfaces;
using BusinessLogic.ViewModels;


namespace FUNewsManagementSystem.Web.Pages.Staff.Categories
{
    [IgnoreAntiforgeryToken]
    public class CreateModel : PageModel
    {
        private readonly ICategoryService _categoryService;


        public CreateModel(ICategoryService categoryService)
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


                if (model == null || string.IsNullOrWhiteSpace(model.CategoryName))
                    return new JsonResult(new { success = false, message = "Invalid category data." });


                var success = await _categoryService.CreateCategoryAsync(model);


                if (!success)
                    return new JsonResult(new { success = false, message = "Category name already exists." });


                return new JsonResult(new { success = true, message = "Category created successfully." });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = "Error: " + ex.Message });
            }
        }
    }
}