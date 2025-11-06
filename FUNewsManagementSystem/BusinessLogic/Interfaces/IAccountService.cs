using DataAccess.Entities;

namespace BusinessLogic.Interfaces
{
    public interface IAccountService
    {
        Task<SystemAccount?> GetAsync(short accountId);
        Task<List<SystemAccount>> GetAllAsync();
        Task<SystemAccount> CreateAsync(string name, string email, int role, string password);
    }
}
