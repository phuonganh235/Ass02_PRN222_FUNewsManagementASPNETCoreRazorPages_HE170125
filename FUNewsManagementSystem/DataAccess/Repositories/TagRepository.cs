using DataAccess.Context;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class TagRepository : GenericRepository<Tag>, ITagRepository
    {
        public TagRepository(FUNewsManagementDbContext context) : base(context)
        {
        }

        public async Task<bool> TagNameExistsAsync(string name, int? excludeTagId = null)
        {
            if (excludeTagId.HasValue)
            {
                return await _dbSet.AnyAsync(t =>
                    t.TagName == name &&
                    t.TagId != excludeTagId.Value);
            }
            return await _dbSet.AnyAsync(t => t.TagName == name);
        }

        public async Task<IEnumerable<Tag>> SearchTagsAsync(string? searchTerm)
        {
            var query = _dbSet.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(t =>
                    t.TagName.ToLower().Contains(searchTerm) ||
                    (t.Note != null && t.Note.ToLower().Contains(searchTerm)));
            }

            return await query.OrderBy(t => t.TagName).ToListAsync();
        }

        public async Task<bool> IsTagUsedAsync(int tagId)
        {
            return await _context.NewsTags.AnyAsync(nt => nt.TagId == tagId);
        }

        public async Task<IEnumerable<Tag>> GetTagsByNewsArticleAsync(string newsArticleId)
        {
            return await _context.NewsTags
                .Where(nt => nt.NewsArticleId == newsArticleId)
                .Select(nt => nt.Tag)
                .ToListAsync();
        }
    }
}