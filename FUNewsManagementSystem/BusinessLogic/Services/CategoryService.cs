using BusinessLogic.Interfaces;
using BusinessLogic.ViewModels;
using DataAccess.Entities;
using DataAccess.Repositories;

namespace BusinessLogic.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<CategoryViewModel>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return categories.Select(MapToViewModel);
        }

        public async Task<IEnumerable<CategoryViewModel>> GetActiveCategoriesAsync()
        {
            var categories = await _categoryRepository.GetActiveCategoriesAsync();
            return categories.Select(MapToViewModel);
        }

        public async Task<IEnumerable<CategoryViewModel>> GetParentCategoriesAsync()
        {
            var categories = await _categoryRepository.GetParentCategoriesAsync();
            return categories.Select(MapToViewModel);
        }

        public async Task<CategoryViewModel?> GetCategoryByIdAsync(short id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            return category != null ? MapToViewModel(category) : null;
        }

        public async Task<bool> CreateCategoryAsync(CategoryViewModel model)
        {
            if (await _categoryRepository.CategoryNameExistsAsync(model.CategoryName))
                return false;

            var category = new Category
            {
                CategoryName = model.CategoryName,
                            CategoryDescription = model.CategoryDesciption,
                ParentCategoryId = model.ParentCategoryId,
                IsActive = model.IsActive
            };

            await _categoryRepository.AddAsync(category);
            return true;
        }

        public async Task<bool> UpdateCategoryAsync(CategoryViewModel model)
        {
            if (await _categoryRepository.CategoryNameExistsAsync(model.CategoryName, model.CategoryId))
                return false;

            var category = await _categoryRepository.GetByIdAsync(model.CategoryId);
            if (category == null) return false;

            category.CategoryName = model.CategoryName;
            category.CategoryDescription = model.CategoryDesciption;
            category.ParentCategoryId = model.ParentCategoryId;
            category.IsActive = model.IsActive;

            await _categoryRepository.UpdateAsync(category);
            return true;
        }

        public async Task<bool> DeleteCategoryAsync(short id)
        {
            if (await _categoryRepository.HasNewsArticlesAsync(id))
                return false;

            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) return false;

            await _categoryRepository.DeleteAsync(category);
            return true;
        }

        public async Task<IEnumerable<CategoryViewModel>> SearchCategoriesAsync(string? searchTerm, bool? isActive)
        {
            var categories = await _categoryRepository.SearchCategoriesAsync(searchTerm, isActive);
            return categories.Select(MapToViewModel);
        }

        public async Task<bool> CanDeleteCategoryAsync(short categoryId)
        {
            return !await _categoryRepository.HasNewsArticlesAsync(categoryId);
        }

        public async Task<bool> CategoryNameExistsAsync(string name, short? excludeCategoryId = null)
        {
            return await _categoryRepository.CategoryNameExistsAsync(name, excludeCategoryId);
        }

        private CategoryViewModel MapToViewModel(Category category)
        {
            return new CategoryViewModel
            {
                CategoryId = category.CategoryID,
                CategoryName = category.CategoryName,
                CategoryDesciption = category.CategoryDescription,
                ParentCategoryId = category.ParentCategoryId,
                ParentCategoryName = category.ParentCategory?.CategoryName,
                IsActive = category.IsActive
            };
        }
    }
}