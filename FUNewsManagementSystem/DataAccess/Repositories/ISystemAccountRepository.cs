using DataAccess.Entities;

namespace DataAccess.Repositories
{
    public interface ISystemAccountRepository : IGenericRepository<SystemAccount>
    {
        Task<SystemAccount?> GetByEmailAsync(string email);
        Task<SystemAccount?> AuthenticateAsync(string email, string password);
        Task<bool> EmailExistsAsync(string email, short? excludeAccountId = null);
        Task<bool> HasNewsArticlesAsync(short accountId);
        Task<IEnumerable<SystemAccount>> SearchAccountsAsync(string? searchTerm, int? role);
    }
}