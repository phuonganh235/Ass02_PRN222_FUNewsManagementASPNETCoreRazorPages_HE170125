using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace FUNewsManagementSystem.Web.Pages.Staff.Categories
{
    public class DeleteModel : PageModel
    {
        private readonly ICategoryService _categoryService;


        public DeleteModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }


        public async Task<JsonResult> OnPostAsync(short id)
        {
            try
            {
                var canDelete = await _categoryService.CanDeleteCategoryAsync(id);
                if (!canDelete)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        message = "This category is currently in use and cannot be deleted."
                    });
                }


                var result = await _categoryService.DeleteCategoryAsync(id);
                if (!result)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        message = "Delete failed. Category may not exist."
                    });
                }


                return new JsonResult(new
                {
                    success = true,
                    message = "Category deleted successfully."
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    success = false,
                    message = "Server exception: " + ex.Message
                });
            }
        }
    }
}