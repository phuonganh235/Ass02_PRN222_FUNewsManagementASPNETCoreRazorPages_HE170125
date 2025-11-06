using BusinessLogic.Interfaces;
using DataAccess.Context;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public class TagService : ITagService
    {
        private readonly FUNewsDbContext _ctx;
        public TagService(FUNewsDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<List<Tag>> GetAllAsync()
        {
            return await _ctx.Tags.OrderBy(t => t.TagName).ToListAsync();
        }

        public async Task<Tag?> GetAsync(int id)
        {
            return await _ctx.Tags.FirstOrDefaultAsync(t => t.TagID == id);
        }

        public async Task<Tag> CreateAsync(string name, string? note)
        {
            var t = new Tag { TagName = name, Note = note };
            _ctx.Tags.Add(t);
            await _ctx.SaveChangesAsync();
            return t;
        }
    }
}
