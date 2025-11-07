using DataAccess.Context;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(FUNewsManagementDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Category>> GetActiveCategoriesAsync()
        {
            return await _dbSet
                .Where(c => c.IsActive)
                .OrderBy(c => c.CategoryName)
                .ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetParentCategoriesAsync()
        {
            return await _dbSet
                .Where(c => c.ParentCategoryID == null && c.IsActive)
                .OrderBy(c => c.CategoryName)
                .ToListAsync();
        }

        public async Task<bool> HasNewsArticlesAsync(short categoryId)
        {
            return await _context.NewsArticles.AnyAsync(n => n.CategoryId == categoryId);
        }

        public async Task<bool> CategoryNameExistsAsync(string name, short? excludeCategoryId = null)
        {
            if (excludeCategoryId.HasValue)
            {
                return await _dbSet.AnyAsync(c =>
                    c.CategoryName == name &&
                    c.CategoryID != excludeCategoryId.Value);
            }
            return await _dbSet.AnyAsync(c => c.CategoryName == name);
        }

        public async Task<IEnumerable<Category>> SearchCategoriesAsync(string? searchTerm, bool? isActive)
        {
            var query = _dbSet.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(c =>
                    c.CategoryName.ToLower().Contains(searchTerm) ||
                    (c.CategoryDescription != null && c.CategoryDescription.ToLower().Contains(searchTerm)));
            }

            if (isActive.HasValue)
            {
                query = query.Where(c => c.IsActive == isActive.Value);
            }

            return await query
                .Include(c => c.ParentCategory)
                .OrderBy(c => c.CategoryName)
                .ToListAsync();
        }

        public async Task<Category?> GetCategoryWithChildrenAsync(short categoryId)
        {
            return await _dbSet
                .Include(c => c.ChildCategories)
                .FirstOrDefaultAsync(c => c.CategoryID == categoryId);
        }
    }
}