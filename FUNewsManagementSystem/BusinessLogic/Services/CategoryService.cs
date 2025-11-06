using BusinessLogic.Interfaces;
using DataAccess.Context;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly FUNewsDbContext _ctx;
        public CategoryService(FUNewsDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<List<Category>> GetAllAsync(bool? onlyActive = null)
        {
            var q = _ctx.Categories.AsQueryable();
            if (onlyActive == true) q = q.Where(x => x.IsActive);
            return await q.OrderBy(x => x.CategoryName).ToListAsync();
        }

        public async Task<Category?> GetAsync(short id)
        {
            return await _ctx.Categories.FirstOrDefaultAsync(x => x.CategoryID == id);
        }

        public async Task<Category> CreateAsync(string name, string? description, short? parentId, bool isActive)
        {
            var c = new Category
            {
                CategoryName = name,
                CategoryDescription = description,
                ParentCategoryID = parentId,
                IsActive = isActive
            };
            _ctx.Categories.Add(c);
            await _ctx.SaveChangesAsync();
            return c;
        }

        public async Task<Category> UpdateAsync(short id, string name, string? description, short? parentId, bool isActive)
        {
            var c = await _ctx.Categories.FindAsync(id) ?? throw new KeyNotFoundException("Category not found");
            c.CategoryName = name;
            c.CategoryDescription = description;
            c.ParentCategoryID = parentId;
            c.IsActive = isActive;
            await _ctx.SaveChangesAsync();
            return c;
        }

        public async Task<bool> DeleteAsync(short id)
        {
            var c = await _ctx.Categories.FindAsync(id);
            if (c == null) return false;
            _ctx.Categories.Remove(c);
            await _ctx.SaveChangesAsync();
            return true;
        }
    }
}
