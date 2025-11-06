using DataAccess.Context;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class SystemAccountRepository : GenericRepository<SystemAccount>, ISystemAccountRepository
    {
        public SystemAccountRepository(FUNewsManagementDbContext context) : base(context)
        {
        }

        public async Task<SystemAccount?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(a => a.AccountEmail == email);
        }

        public async Task<SystemAccount?> AuthenticateAsync(string email, string password)
        {
            return await _dbSet.FirstOrDefaultAsync(a =>
                a.AccountEmail == email &&
                a.AccountPassword == password);
        }

        public async Task<bool> EmailExistsAsync(string email, short? excludeAccountId = null)
        {
            if (excludeAccountId.HasValue)
            {
                return await _dbSet.AnyAsync(a =>
                    a.AccountEmail == email &&
                    a.AccountId != excludeAccountId.Value);
            }
            return await _dbSet.AnyAsync(a => a.AccountEmail == email);
        }

        public async Task<bool> HasNewsArticlesAsync(short accountId)
        {
            return await _context.NewsArticles.AnyAsync(n => n.CreatedById == accountId);
        }

        public async Task<IEnumerable<SystemAccount>> SearchAccountsAsync(string? searchTerm, int? role)
        {
            var query = _dbSet.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(a =>
                    a.AccountName.ToLower().Contains(searchTerm) ||
                    a.AccountEmail.ToLower().Contains(searchTerm));
            }

            if (role.HasValue)
            {
                query = query.Where(a => a.AccountRole == role.Value);
            }

            return await query.OrderBy(a => a.AccountName).ToListAsync();
        }
    }
}