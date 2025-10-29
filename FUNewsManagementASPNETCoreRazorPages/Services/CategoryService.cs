using BusinessObjects;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(short id);
        Task<bool> AddAsync(Category cat);
        Task<bool> UpdateAsync(Category cat);
        Task<bool> DeleteAsync(short id);
        Task<IEnumerable<Category>> SearchAsync(string keyword);
    }

    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Category> _categoryRepo;
        private readonly IGenericRepository<NewsArticle> _newsRepo;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(
            IGenericRepository<Category> categoryRepo,
            IGenericRepository<NewsArticle> newsRepo,
            ILogger<CategoryService> logger)
        {
            _categoryRepo = categoryRepo;
            _newsRepo = newsRepo;
            _logger = logger;
        }

        public async Task<IEnumerable<Category>> GetAllAsync() => await _categoryRepo.GetAllAsync();

        public async Task<Category?> GetByIdAsync(short id)
        {
            var list = await _categoryRepo.GetByPredicateAsync(x => x.CategoryId == id);
            return list.FirstOrDefault();
        }

        public async Task<bool> AddAsync(Category cat)
        {
            if (string.IsNullOrWhiteSpace(cat.CategoryName)) return false;
            return await _categoryRepo.AddAsync(cat);
        }

        public async Task<bool> UpdateAsync(Category cat)
        {
            return await _categoryRepo.UpdateAsync(cat);
        }

        public async Task<bool> DeleteAsync(short id)
        {
            var used = await _newsRepo.GetByPredicateAsync(x => x.CategoryId == id);
            if (used.Any()) return false;
            return await _categoryRepo.DeleteAsync(id);
        }

        public async Task<IEnumerable<Category>> SearchAsync(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return await _categoryRepo.GetAllAsync();

            return await _categoryRepo.GetByPredicateAsync(x =>
                x.CategoryName.Contains(keyword) ||
                x.CategoryDesciption.Contains(keyword));
        }
    }
}
