using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<T?> GetByIdAsync(object id, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> GetByPredicateAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        Task<(IEnumerable<T> Items, int TotalCount)> GetByPageAsync(
            IEnumerable<Expression<Func<T, bool>>>? filters = null,
            Expression<Func<T, object>>? orderBy = null,
            bool descending = false,
            string? search = null,
            Expression<Func<T, string>>? searchExpression = null,
            int pageNumber = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default);

        Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(object id, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        Task<bool> AddAsync(T entity, CancellationToken cancellationToken = default);
        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

        Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken = default);

        Task<bool> DeleteAsync(object id, CancellationToken cancellationToken = default);
        Task DeleteRangeAsync(IEnumerable<object> ids, CancellationToken cancellationToken = default);
    }

}
