using System.Linq.Expressions;

namespace DataAccess.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(object id);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task DeleteRangeAsync(IEnumerable<T> entities);
        Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    }
}