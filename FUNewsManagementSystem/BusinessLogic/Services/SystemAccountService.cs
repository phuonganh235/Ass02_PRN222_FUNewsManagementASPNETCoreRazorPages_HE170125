using BusinessLogic.Interfaces;
using BusinessLogic.ViewModels;
using DataAccess.Entities;
using DataAccess.Repositories;

namespace BusinessLogic.Services
{
    public class SystemAccountService : ISystemAccountService
    {
        private readonly ISystemAccountRepository _accountRepository;

        public SystemAccountService(ISystemAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<AccountViewModel?> AuthenticateAsync(string email, string password)
        {
            var account = await _accountRepository.AuthenticateAsync(email, password);
            if (account == null) return null;

            return MapToViewModel(account);
        }

        public async Task<IEnumerable<AccountViewModel>> GetAllAccountsAsync()
        {
            var accounts = await _accountRepository.GetAllAsync();
            return accounts.Select(MapToViewModel);
        }

        public async Task<AccountViewModel?> GetAccountByIdAsync(short id)
        {
            var account = await _accountRepository.GetByIdAsync(id);
            return account != null ? MapToViewModel(account) : null;
        }

        public async Task<AccountViewModel?> GetAccountByEmailAsync(string email)
        {
            var account = await _accountRepository.GetByEmailAsync(email);
            return account != null ? MapToViewModel(account) : null;
        }

        public async Task<bool> CreateAccountAsync(AccountViewModel model)
        {
            if (await _accountRepository.EmailExistsAsync(model.AccountEmail))
                return false;

            var account = new SystemAccount
            {
                AccountName = model.AccountName,
                AccountEmail = model.AccountEmail,
                AccountRole = model.AccountRole,
                AccountPassword = model.AccountPassword ?? string.Empty
            };

            await _accountRepository.AddAsync(account);
            return true;
        }

        public async Task<bool> UpdateAccountAsync(AccountViewModel model)
        {
            if (await _accountRepository.EmailExistsAsync(model.AccountEmail, model.AccountId))
                return false;

            var account = await _accountRepository.GetByIdAsync(model.AccountId);
            if (account == null) return false;

            account.AccountName = model.AccountName;
            account.AccountEmail = model.AccountEmail;
            account.AccountRole = model.AccountRole;

            if (!string.IsNullOrEmpty(model.AccountPassword))
            {
                account.AccountPassword = model.AccountPassword;
            }

            await _accountRepository.UpdateAsync(account);
            return true;
        }

        public async Task<bool> DeleteAccountAsync(short id)
        {
            if (await _accountRepository.HasNewsArticlesAsync(id))
                return false;

            var account = await _accountRepository.GetByIdAsync(id);
            if (account == null) return false;

            await _accountRepository.DeleteAsync(account);
            return true;
        }

        public async Task<IEnumerable<AccountViewModel>> SearchAccountsAsync(string? searchTerm, int? role)
        {
            var accounts = await _accountRepository.SearchAccountsAsync(searchTerm, role);
            return accounts.Select(MapToViewModel);
        }

        public async Task<bool> EmailExistsAsync(string email, short? excludeAccountId = null)
        {
            return await _accountRepository.EmailExistsAsync(email, excludeAccountId);
        }

        public async Task<bool> CanDeleteAccountAsync(short accountId)
        {
            return !await _accountRepository.HasNewsArticlesAsync(accountId);
        }

        private AccountViewModel MapToViewModel(SystemAccount account)
        {
            return new AccountViewModel
            {
                AccountId = account.AccountId,
                AccountName = account.AccountName,
                AccountEmail = account.AccountEmail,
                AccountRole = account.AccountRole
            };
        }
    }
}