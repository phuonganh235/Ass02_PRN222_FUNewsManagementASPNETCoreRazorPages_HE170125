using BusinessLogic.ViewModels;

namespace BusinessLogic.Interfaces
{
    public interface ITagService
    {
        Task<IEnumerable<TagViewModel>> GetAllTagsAsync();
        Task<TagViewModel?> GetTagByIdAsync(int id);
        Task<bool> CreateTagAsync(TagViewModel model);
        Task<bool> UpdateTagAsync(TagViewModel model);
        Task<bool> DeleteTagAsync(int id);
        Task<IEnumerable<TagViewModel>> SearchTagsAsync(string? searchTerm);
        Task<bool> TagNameExistsAsync(string name, int? excludeTagId = null);
        Task<bool> CanDeleteTagAsync(int tagId);
    }
}