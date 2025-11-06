using BusinessLogic.ViewModels;

namespace BusinessLogic.Interfaces
{
    public interface ISystemAccountService
    {
        Task<AccountViewModel?> AuthenticateAsync(string email, string password);
        Task<IEnumerable<AccountViewModel>> GetAllAccountsAsync();
        Task<AccountViewModel?> GetAccountByIdAsync(short id);
        Task<AccountViewModel?> GetAccountByEmailAsync(string email);
        Task<bool> CreateAccountAsync(AccountViewModel model);
        Task<bool> UpdateAccountAsync(AccountViewModel model);
        Task<bool> DeleteAccountAsync(short id);
        Task<IEnumerable<AccountViewModel>> SearchAccountsAsync(string? searchTerm, int? role);
        Task<bool> EmailExistsAsync(string email, short? excludeAccountId = null);
        Task<bool> CanDeleteAccountAsync(short accountId);
    }
}