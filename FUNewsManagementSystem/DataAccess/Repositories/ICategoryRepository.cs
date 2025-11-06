using DataAccess.Entities;

namespace DataAccess.Repositories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<IEnumerable<Category>> GetActiveCategoriesAsync();
        Task<IEnumerable<Category>> GetParentCategoriesAsync();
        Task<bool> HasNewsArticlesAsync(short categoryId);
        Task<bool> CategoryNameExistsAsync(string name, short? excludeCategoryId = null);
        Task<IEnumerable<Category>> SearchCategoriesAsync(string? searchTerm, bool? isActive);
        Task<Category?> GetCategoryWithChildrenAsync(short categoryId);
    }
}