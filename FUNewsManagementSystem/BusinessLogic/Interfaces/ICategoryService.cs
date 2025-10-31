using BusinessLogic.ViewModels;

namespace BusinessLogic.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryVM>> GetAllAsync();
        Task<CategoryVM?> GetByIdAsync(int id);
        Task<bool> CreateAsync(CategoryVM model);
        Task<bool> UpdateAsync(CategoryVM model);
        Task<bool> DeleteAsync(int id);
    }
}
