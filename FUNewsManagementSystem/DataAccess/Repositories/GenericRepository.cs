using DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccess.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly FUNewsDbContext _ctx;
        protected readonly DbSet<T> _db;

        public GenericRepository(FUNewsDbContext ctx)
        {
            _ctx = ctx;
            _db = ctx.Set<T>();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _db.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(object id)
        {
            return await _db.FindAsync(id);
        }

        public async Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _db.Where(predicate).ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _db.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _db.Update(entity);
        }

        public void Delete(T entity)
        {
            _db.Remove(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _ctx.SaveChangesAsync();
        }
    }
}
