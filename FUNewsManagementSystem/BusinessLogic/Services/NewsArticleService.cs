using BusinessLogic.Interfaces;
using BusinessLogic.ViewModels;
using DataAccess.Entities;
using DataAccess.Repositories;

namespace BusinessLogic.Services
{
    public class NewsArticleService : INewsArticleService
    {
        private readonly INewsArticleRepository _newsRepository;
        private readonly INewsTagRepository _newsTagRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ISystemAccountRepository _accountRepository;

        public NewsArticleService(
            INewsArticleRepository newsRepository,
            INewsTagRepository newsTagRepository,
            ICategoryRepository categoryRepository,
            ISystemAccountRepository accountRepository)
        {
            _newsRepository = newsRepository;
            _newsTagRepository = newsTagRepository;
            _categoryRepository = categoryRepository;
            _accountRepository = accountRepository;
        }

        public async Task<IEnumerable<NewsArticleViewModel>> GetAllNewsAsync()
        {
            var news = await _newsRepository.GetAllAsync();
            var viewModels = new List<NewsArticleViewModel>();

            foreach (var item in news)
            {
                viewModels.Add(await MapToViewModel(item));
            }

            return viewModels;
        }

        public async Task<IEnumerable<NewsArticleViewModel>> GetActiveNewsAsync()
        {
            var news = await _newsRepository.GetActiveNewsAsync();
            var viewModels = new List<NewsArticleViewModel>();

            foreach (var item in news)
            {
                viewModels.Add(await MapToViewModel(item));
            }

            return viewModels;
        }

        public async Task<IEnumerable<NewsArticleViewModel>> GetNewsByAuthorAsync(short authorId)
        {
            var news = await _newsRepository.GetNewsByAuthorAsync(authorId);
            var viewModels = new List<NewsArticleViewModel>();

            foreach (var item in news)
            {
                viewModels.Add(await MapToViewModel(item));
            }

            return viewModels;
        }

        public async Task<IEnumerable<NewsArticleViewModel>> GetNewsByCategoryAsync(short categoryId)
        {
            var news = await _newsRepository.GetNewsByCategoryAsync(categoryId);
            var viewModels = new List<NewsArticleViewModel>();

            foreach (var item in news)
            {
                viewModels.Add(await MapToViewModel(item));
            }

            return viewModels;
        }

        public async Task<NewsArticleViewModel?> GetNewsByIdAsync(string newsId)
        {
            var news = await _newsRepository.GetNewsWithDetailsAsync(newsId);
            return news != null ? await MapToViewModel(news) : null;
        }

        public async Task<bool> CreateNewsAsync(NewsArticleViewModel model, short createdById)
        {
            if (await _newsRepository.NewsIdExistsAsync(model.NewsArticleId))
                return false;

            var newsArticle = new NewsArticle
            {
                NewsArticleId = model.NewsArticleId,
                NewsTitle = model.NewsTitle,
                Headline = model.Headline,
                NewsContent = model.NewsContent,
                NewsSource = model.NewsSource,
                CategoryId = model.CategoryId,
                NewsStatus = model.NewsStatus,
                CreatedDate = DateTime.Now,
                CreatedById = createdById
            };

            await _newsRepository.AddAsync(newsArticle);

            // Add tags
            if (model.SelectedTagIds.Any())
            {
                await _newsTagRepository.AddTagsToNewsAsync(model.NewsArticleId, model.SelectedTagIds);
            }

            return true;
        }

        public async Task<bool> UpdateNewsAsync(NewsArticleViewModel model, short updatedById)
        {
            var newsArticle = await _newsRepository.GetByIdAsync(model.NewsArticleId);
            if (newsArticle == null) return false;

            newsArticle.NewsTitle = model.NewsTitle;
            newsArticle.Headline = model.Headline;
            newsArticle.NewsContent = model.NewsContent;
            newsArticle.NewsSource = model.NewsSource;
            newsArticle.CategoryId = model.CategoryId;
            newsArticle.NewsStatus = model.NewsStatus;
            newsArticle.UpdatedById = updatedById;
            newsArticle.ModifiedDate = DateTime.Now;

            await _newsRepository.UpdateAsync(newsArticle);

            // Update tags
            await _newsTagRepository.UpdateNewsTagsAsync(model.NewsArticleId, model.SelectedTagIds);

            return true;
        }

        public async Task<bool> DeleteNewsAsync(string newsId)
        {
            var newsArticle = await _newsRepository.GetByIdAsync(newsId);
            if (newsArticle == null) return false;

            await _newsRepository.DeleteAsync(newsArticle);
            return true;
        }

        public async Task<IEnumerable<NewsArticleViewModel>> SearchNewsAsync(
            string? searchTerm,
            short? categoryId,
            bool? status,
            DateTime? fromDate,
            DateTime? toDate)
        {
            var news = await _newsRepository.SearchNewsAsync(searchTerm, categoryId, status, fromDate, toDate);
            var viewModels = new List<NewsArticleViewModel>();

            foreach (var item in news)
            {
                viewModels.Add(await MapToViewModel(item));
            }

            return viewModels;
        }

        public async Task<IEnumerable<NewsArticleViewModel>> GetRelatedNewsAsync(string newsId, int count = 5)
        {
            var news = await _newsRepository.GetRelatedNewsAsync(newsId, count);
            var viewModels = new List<NewsArticleViewModel>();

            foreach (var item in news)
            {
                viewModels.Add(await MapToViewModel(item));
            }

            return viewModels;
        }

        public async Task<bool> NewsIdExistsAsync(string newsId)
        {
            return await _newsRepository.NewsIdExistsAsync(newsId);
        }

        public async Task<Dictionary<string, int>> GetArticleStatisticsByCategoryAsync(DateTime? startDate, DateTime? endDate)
        {
            var stats = await _newsRepository.GetArticleCountByCategoryAsync(startDate, endDate);
            var result = new Dictionary<string, int>();

            foreach (var stat in stats)
            {
                var category = await _categoryRepository.GetByIdAsync(stat.Key);
                if (category != null)
                {
                    result[category.CategoryName] = stat.Value;
                }
            }

            return result;
        }

        public async Task<Dictionary<string, int>> GetArticleStatisticsByAuthorAsync(DateTime? startDate, DateTime? endDate)
        {
            var stats = await _newsRepository.GetArticleCountByAuthorAsync(startDate, endDate);
            var result = new Dictionary<string, int>();

            foreach (var stat in stats)
            {
                var author = await _accountRepository.GetByIdAsync(stat.Key);
                if (author != null)
                {
                    result[author.AccountName] = stat.Value;
                }
            }

            return result;
        }

        private async Task<NewsArticleViewModel> MapToViewModel(NewsArticle news)
        {
            var viewModel = new NewsArticleViewModel
            {
                NewsArticleId = news.NewsArticleId,
                NewsTitle = news.NewsTitle,
                Headline = news.Headline,
                NewsContent = news.NewsContent,
                NewsSource = news.NewsSource,
                CategoryId = news.CategoryId,
                CategoryName = news.Category?.CategoryName,
                NewsStatus = news.NewsStatus,
                CreatedDate = news.CreatedDate,
                CreatedById = news.CreatedById,
                CreatedByName = news.CreatedBy?.AccountName,
                UpdatedById = news.UpdatedById,
                UpdatedByName = news.UpdatedBy?.AccountName,
                ModifiedDate = news.ModifiedDate
            };

            // Get tags
            var newsTags = await _newsTagRepository.GetByNewsArticleIdAsync(news.NewsArticleId);
            viewModel.Tags = newsTags.Select(nt => new TagViewModel
            {
                TagId = nt.Tag.TagId,
                TagName = nt.Tag.TagName,
                Note = nt.Tag.Note
            }).ToList();

            viewModel.SelectedTagIds = viewModel.Tags.Select(t => t.TagId).ToList();

            return viewModel;
        }
    }
}