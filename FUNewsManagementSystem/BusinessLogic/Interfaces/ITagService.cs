using BusinessLogic.ViewModels;

namespace BusinessLogic.Interfaces
{
    public interface ITagService
    {
        Task<List<TagVM>> GetAllAsync();
        Task<TagVM?> GetByIdAsync(int id);
        Task<bool> CreateAsync(TagVM vm);
        Task<bool> UpdateAsync(TagVM vm);
        Task<bool> DeleteAsync(int id);
        Task<bool> IsDuplicateAsync(string name, int? excludeId = null);
    }
}
