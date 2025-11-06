using BusinessLogic.Interfaces;
using BusinessLogic.ViewModels;
using DataAccess.Entities;
using DataAccess.Repositories;

namespace BusinessLogic.Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;

        public TagService(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public async Task<IEnumerable<TagViewModel>> GetAllTagsAsync()
        {
            var tags = await _tagRepository.GetAllAsync();
            return tags.Select(MapToViewModel);
        }

        public async Task<TagViewModel?> GetTagByIdAsync(int id)
        {
            var tag = await _tagRepository.GetByIdAsync(id);
            return tag != null ? MapToViewModel(tag) : null;
        }

        public async Task<bool> CreateTagAsync(TagViewModel model)
        {
            if (await _tagRepository.TagNameExistsAsync(model.TagName))
                return false;

            var tag = new Tag
            {
                TagName = model.TagName,
                Note = model.Note
            };

            await _tagRepository.AddAsync(tag);
            return true;
        }

        public async Task<bool> UpdateTagAsync(TagViewModel model)
        {
            if (await _tagRepository.TagNameExistsAsync(model.TagName, model.TagId))
                return false;

            var tag = await _tagRepository.GetByIdAsync(model.TagId);
            if (tag == null) return false;

            tag.TagName = model.TagName;
            tag.Note = model.Note;

            await _tagRepository.UpdateAsync(tag);
            return true;
        }

        public async Task<bool> DeleteTagAsync(int id)
        {
            var tag = await _tagRepository.GetByIdAsync(id);
            if (tag == null) return false;

            await _tagRepository.DeleteAsync(tag);
            return true;
        }

        public async Task<IEnumerable<TagViewModel>> SearchTagsAsync(string? searchTerm)
        {
            var tags = await _tagRepository.SearchTagsAsync(searchTerm);
            return tags.Select(MapToViewModel);
        }

        public async Task<bool> TagNameExistsAsync(string name, int? excludeTagId = null)
        {
            return await _tagRepository.TagNameExistsAsync(name, excludeTagId);
        }

        public async Task<bool> CanDeleteTagAsync(int tagId)
        {
            return !await _tagRepository.IsTagUsedAsync(tagId);
        }

        private TagViewModel MapToViewModel(Tag tag)
        {
            return new TagViewModel
            {
                TagId = tag.TagId,
                TagName = tag.TagName,
                Note = tag.Note
            };
        }
    }
}