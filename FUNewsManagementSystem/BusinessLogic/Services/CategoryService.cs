using BusinessLogic.Interfaces;
using BusinessLogic.ViewModels;
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

        public async Task<List<CategoryVM>> GetAllAsync()
        {
            return await _ctx.Categories
                .Select(c => new CategoryVM
                {
                    CategoryID = c.CategoryID,
                    CategoryName = c.CategoryName,
                    CategoryDescription = c.CategoryDescription,
                    ParentCategoryID = c.ParentCategoryID,
                    IsActive = c.IsActive
                })
                .ToListAsync();
        }

        public async Task<CategoryVM?> GetByIdAsync(int id)
        {
            return await _ctx.Categories
                .Where(c => c.CategoryID == id)
                .Select(c => new CategoryVM
                {
                    CategoryID = c.CategoryID,
                    CategoryName = c.CategoryName,
                    CategoryDescription = c.CategoryDescription,
                    ParentCategoryID = c.ParentCategoryID,
                    IsActive = c.IsActive
                })
                .FirstOrDefaultAsync();
        }

        public async Task<bool> CreateAsync(CategoryVM model)
        {
            var c = new Category
            {
                CategoryName = model.CategoryName,
                CategoryDescription = model.CategoryDescription,
                ParentCategoryID = model.ParentCategoryID,
                IsActive = model.IsActive
            };
            _ctx.Categories.Add(c);
            await _ctx.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(CategoryVM model)
        {
            var c = await _ctx.Categories.FindAsync(model.CategoryID);
            if (c == null) return false;

            c.CategoryName = model.CategoryName;
            c.CategoryDescription = model.CategoryDescription;
            c.ParentCategoryID = model.ParentCategoryID;
            c.IsActive = model.IsActive;

            _ctx.Categories.Update(c);
            await _ctx.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var c = await _ctx.Categories.FindAsync(id);
            if (c == null) return false;

            _ctx.Categories.Remove(c);
            await _ctx.SaveChangesAsync();
            return true;
        }
    }
}
