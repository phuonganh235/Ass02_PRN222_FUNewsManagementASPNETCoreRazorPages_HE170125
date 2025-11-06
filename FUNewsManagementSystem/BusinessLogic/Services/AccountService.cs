using BusinessLogic.Interfaces;
using BusinessLogic.ViewModels;
using DataAccess.Entities;
using DataAccess.Repositories;

namespace BusinessLogic.Services
{
    /// <summary>
    /// Account service implementation
    /// Handles all account-related business logic
    /// </summary>
    public class AccountService : IAccountService
    {
        private readonly ISystemAccountRepository _accountRepository;

        public AccountService(ISystemAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        /// <summary>
        /// Authenticate user with email and password
        /// </summary>
        public async Task<AccountViewModel?> AuthenticateAsync(string email, string password)
        {
            var account = await _accountRepository.AuthenticateAsync(email, password);
            if (account == null) return null;

            return MapToViewModel(account);
        }

        /// <summary>
        /// Get all accounts from database
        /// </summary>
        public async Task<IEnumerable<AccountViewModel>> GetAllAccountsAsync()
        {
            var accounts = await _accountRepository.GetAllAsync();
            return accounts.Select(MapToViewModel);
        }

        /// <summary>
        /// Get account by ID
        /// </summary>
        public async Task<AccountViewModel?> GetAccountByIdAsync(short id)
        {
            var account = await _accountRepository.GetByIdAsync(id);
            return account != null ? MapToViewModel(account) : null;
        }

        /// <summary>
        /// Get account by email address
        /// </summary>
        public async Task<AccountViewModel?> GetAccountByEmailAsync(string email)
        {
            var account = await _accountRepository.GetByEmailAsync(email);
            return account != null ? MapToViewModel(account) : null;
        }

        /// <summary>
        /// Create new account
        /// </summary>
        public async Task<bool> CreateAccountAsync(AccountViewModel model)
        {
            // Check if email already exists
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

        /// <summary>
        /// Update existing account
        /// </summary>
        public async Task<bool> UpdateAccountAsync(AccountViewModel model)
        {
            // Check if email already exists (excluding current account)
            if (await _accountRepository.EmailExistsAsync(model.AccountEmail, model.AccountId))
                return false;

            var account = await _accountRepository.GetByIdAsync(model.AccountId);
            if (account == null) return false;

            // Update account properties
            account.AccountName = model.AccountName;
            account.AccountEmail = model.AccountEmail;
            account.AccountRole = model.AccountRole;

            // Only update password if provided
            if (!string.IsNullOrEmpty(model.AccountPassword))
            {
                account.AccountPassword = model.AccountPassword;
            }

            await _accountRepository.UpdateAsync(account);
            return true;
        }

        /// <summary>
        /// Delete account (only if no news articles)
        /// </summary>
        public async Task<bool> DeleteAccountAsync(short id)
        {
            // Check if account has created any news articles
            if (await _accountRepository.HasNewsArticlesAsync(id))
                return false;

            var account = await _accountRepository.GetByIdAsync(id);
            if (account == null) return false;

            await _accountRepository.DeleteAsync(account);
            return true;
        }

        /// <summary>
        /// Search accounts by name/email and role
        /// </summary>
        public async Task<IEnumerable<AccountViewModel>> SearchAccountsAsync(string? searchTerm, int? role)
        {
            var accounts = await _accountRepository.SearchAccountsAsync(searchTerm, role);
            return accounts.Select(MapToViewModel);
        }

        /// <summary>
        /// Check if email exists in database
        /// </summary>
        public async Task<bool> EmailExistsAsync(string email, short? excludeAccountId = null)
        {
            return await _accountRepository.EmailExistsAsync(email, excludeAccountId);
        }

        /// <summary>
        /// Check if account can be safely deleted
        /// </summary>
        public async Task<bool> CanDeleteAccountAsync(short accountId)
        {
            return !await _accountRepository.HasNewsArticlesAsync(accountId);
        }

        /// <summary>
        /// Map SystemAccount entity to AccountViewModel
        /// </summary>
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