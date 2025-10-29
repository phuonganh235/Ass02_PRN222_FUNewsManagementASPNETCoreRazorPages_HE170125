using BusinessObjects;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface ITagService
    {
        Task<IEnumerable<Tag>> GetAllAsync();
        Task<Tag?> GetByIdAsync(int id);
        Task<bool> AddAsync(Tag tag);
        Task<bool> UpdateAsync(Tag tag);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Tag>> SearchAsync(string keyword);
        Task<bool> IsDuplicateAsync(string tagName, int? excludeId = null);
    }

    public class TagService : ITagService
    {
        private readonly IGenericRepository<Tag> _tagRepo;
        private readonly IGenericRepository<NewsTag> _newsTagRepo;
        private readonly ILogger<TagService> _logger;

        public TagService(
            IGenericRepository<Tag> tagRepo,
            IGenericRepository<NewsTag> newsTagRepo,
            ILogger<TagService> logger)
        {
            _tagRepo = tagRepo;
            _newsTagRepo = newsTagRepo;
            _logger = logger;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync() => await _tagRepo.GetAllAsync();

        public async Task<Tag?> GetByIdAsync(int id)
        {
            var tags = await _tagRepo.GetByPredicateAsync(x => x.TagId == id);
            return tags.FirstOrDefault();
        }

        public async Task<bool> AddAsync(Tag tag)
        {
            if (string.IsNullOrWhiteSpace(tag.TagName)) return false;
            if (await IsDuplicateAsync(tag.TagName)) return false;
            return await _tagRepo.AddAsync(tag);
        }

        public async Task<bool> UpdateAsync(Tag tag)
        {
            if (await IsDuplicateAsync(tag.TagName, tag.TagId)) return false;
            return await _tagRepo.UpdateAsync(tag);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var used = await _newsTagRepo.GetByPredicateAsync(x => x.TagId == id);
            if (used.Any()) return false;
            return await _tagRepo.DeleteAsync(id);
        }

        public async Task<IEnumerable<Tag>> SearchAsync(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return await _tagRepo.GetAllAsync();

            return await _tagRepo.GetByPredicateAsync(x => x.TagName.Contains(keyword));
        }

        public async Task<bool> IsDuplicateAsync(string tagName, int? excludeId = null)
        {
            var all = await _tagRepo.GetByPredicateAsync(x => x.TagName.ToLower() == tagName.ToLower());
            if (excludeId.HasValue)
                return all.Any(x => x.TagId != excludeId.Value);
            return all.Any();
        }
    }
}
