using BusinessLogic.Interfaces;
using BusinessLogic.ViewModels;
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

        public async Task<List<TagVM>> GetAllAsync()
        {
            return await _ctx.Tags
                .Select(t => new TagVM
                {
                    TagID = t.TagID,
                    TagName = t.TagName
                })
                .ToListAsync();
        }

        public async Task<TagVM?> GetByIdAsync(int id)
        {
            return await _ctx.Tags
                .Where(t => t.TagID == id)
                .Select(t => new TagVM
                {
                    TagID = t.TagID,
                    TagName = t.TagName
                })
                .FirstOrDefaultAsync();
        }

        public async Task<bool> IsDuplicateAsync(string name, int? excludeId = null)
        {
            return await _ctx.Tags.AnyAsync(t =>
                t.TagName == name &&
                (!excludeId.HasValue || t.TagID != excludeId.Value)
            );
        }

        public async Task<bool> CreateAsync(TagVM vm)
        {
            if (await IsDuplicateAsync(vm.TagName ?? "")) return false;

            var t = new Tag
            {
                TagName = vm.TagName
            };
            _ctx.Tags.Add(t);
            await _ctx.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(TagVM vm)
        {
            var t = await _ctx.Tags.FindAsync(vm.TagID);
            if (t == null) return false;

            if (await IsDuplicateAsync(vm.TagName ?? "", vm.TagID)) return false;

            t.TagName = vm.TagName;
            _ctx.Tags.Update(t);
            await _ctx.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var t = await _ctx.Tags.FindAsync(id);
            if (t == null) return false;

            _ctx.Tags.Remove(t);
            await _ctx.SaveChangesAsync();
            return true;
        }
    }
}
