using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using Repositories;
using System.Linq.Expressions;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly DbContext _context;
    private readonly DbSet<T> _dbSet;

    // AppDbContext của bạn phải kế thừa DbContext
    public GenericRepository(DbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _dbSet = _context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<T?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        // FindAsync nhận object[] keys
        var entity = await _dbSet.FindAsync(new[] { id }, cancellationToken);
        if (entity == null)
        {
            // nếu key là composite, caller nên tự viết repo riêng
            return null;
        }
        return entity;
    }

    public async Task<IEnumerable<T>> GetByPredicateAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsNoTracking().Where(predicate).ToListAsync(cancellationToken);
    }

    public async Task<(IEnumerable<T> Items, int TotalCount)> GetByPageAsync(
        IEnumerable<Expression<Func<T, bool>>>? filters = null,
        Expression<Func<T, object>>? orderBy = null,
        bool descending = false,
        string? search = null,
        Expression<Func<T, string>>? searchExpression = null,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        if (pageNumber <= 0) pageNumber = 1;
        if (pageSize <= 0) pageSize = 10;

        IQueryable<T> query = _dbSet.AsNoTracking();

        // Filters
        if (filters != null)
        {
            foreach (var f in filters)
            {
                query = query.Where(f);
            }
        }

        // Search
        if (!string.IsNullOrWhiteSpace(search) && searchExpression != null)
        {
            var param = Expression.Parameter(typeof(T), "x");
            // x => searchExpression(x).Contains(search)
            var body = Expression.Call(
                searchExpression.Body,
                typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) })!,
                Expression.Constant(search)
            );
            var containsLambda = Expression.Lambda<Func<T, bool>>(body, searchExpression.Parameters);
            query = query.Where(containsLambda);
        }

        // Count trước khi phân trang
        var total = await query.CountAsync(cancellationToken);

        // Order
        if (orderBy != null)
        {
            query = descending ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
        }

        // Paging
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, total);
    }

    public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        if (predicate == null)
            return await _dbSet.CountAsync(cancellationToken);

        return await _dbSet.CountAsync(predicate, cancellationToken);
    }

    public async Task<bool> ExistsAsync(object id, CancellationToken cancellationToken = default)
    {
        var entity = await _dbSet.FindAsync(new[] { id }, cancellationToken);
        return entity != null;
    }

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(predicate, cancellationToken);
    }

    public async Task<bool> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
        return await SaveAsync(cancellationToken) > 0;
    }

    public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddRangeAsync(entities, cancellationToken);
        await SaveAsync(cancellationToken);
    }

    public async Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
        return await SaveAsync(cancellationToken) > 0;
    }

    public async Task<bool> DeleteAsync(object id, CancellationToken cancellationToken = default)
    {
        var entity = await _dbSet.FindAsync(new[] { id }, cancellationToken);
        if (entity == null) return false;

        _dbSet.Remove(entity);
        return await SaveAsync(cancellationToken) > 0;
    }

    public async Task DeleteRangeAsync(IEnumerable<object> ids, CancellationToken cancellationToken = default)
    {
        foreach (var id in ids)
        {
            var entity = await _dbSet.FindAsync(new[] { id }, cancellationToken);
            if (entity != null) _dbSet.Remove(entity);
        }
        await SaveAsync(cancellationToken);
    }

    private async Task<int> SaveAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            // có thể log chi tiết ở đây
            throw;
        }
        catch (DbUpdateException)
        {
            // có thể log chi tiết ở đây
            throw;
        }
    }
}
