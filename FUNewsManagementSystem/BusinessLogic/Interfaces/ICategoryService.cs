using BusinessLogic.ViewModels;

namespace BusinessLogic.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryViewModel>> GetAllCategoriesAsync();
        Task<IEnumerable<CategoryViewModel>> GetActiveCategoriesAsync();
        Task<IEnumerable<CategoryViewModel>> GetParentCategoriesAsync();
        Task<CategoryViewModel?> GetCategoryByIdAsync(short id);
        Task<bool> CreateCategoryAsync(CategoryViewModel model);
        Task<bool> UpdateCategoryAsync(CategoryViewModel model);
        Task<bool> DeleteCategoryAsync(short id);
        Task<IEnumerable<CategoryViewModel>> SearchCategoriesAsync(string? searchTerm, bool? isActive);
        Task<bool> CanDeleteCategoryAsync(short categoryId);
        Task<bool> CategoryNameExistsAsync(string name, short? excludeCategoryId = null);
        Task<bool> UpdateCategoryOrdersAsync(Dictionary<short, int> categoryOrders);
    }
}