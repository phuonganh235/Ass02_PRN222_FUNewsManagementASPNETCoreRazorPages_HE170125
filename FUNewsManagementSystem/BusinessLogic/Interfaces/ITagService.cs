using DataAccess.Entities;

namespace BusinessLogic.Interfaces
{
    public interface ITagService
    {
        Task<List<Tag>> GetAllAsync();
        Task<Tag?> GetAsync(int id);
        Task<Tag> CreateAsync(string name, string? note);
    }
}
