using BusinessLogic.ViewModels;

namespace BusinessLogic.Interfaces
{
    public interface IAccountService
    {
        Task<List<AccountVM>> GetAllAsync();
        Task<AccountVM?> GetByIdAsync(int id);
        Task<bool> CreateAsync(AccountVM model);
        Task<bool> UpdateAsync(AccountVM model);
        Task<bool> DeleteAsync(int id);
    }
}
