using DataAccess.Entities;

namespace BusinessLogic.Interfaces
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllAsync(bool? onlyActive = null);
        Task<Category?> GetAsync(short id);
        Task<Category> CreateAsync(string name, string? description, short? parentId, bool isActive);
        Task<Category> UpdateAsync(short id, string name, string? description, short? parentId, bool isActive);
        Task<bool> DeleteAsync(short id);
    }
}
