using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface INewsService
    {
        Task<IEnumerable<NewsArticle>> GetAllAsync(bool onlyActive = false);
        Task<NewsArticle?> GetAsync(string id);
        Task<(bool ok, string message)> CreateAsync(NewsArticle entity, List<int> tagIds);
        Task<(bool ok, string message)> UpdateAsync(NewsArticle entity, List<int> tagIds);
        Task<(bool ok, string message)> DeleteAsync(string id);
        Task<(bool ok, string message)> DuplicateAsync(string id);
        Task<IEnumerable<NewsArticle>> GetRelatedAsync(string id);
    }
}
