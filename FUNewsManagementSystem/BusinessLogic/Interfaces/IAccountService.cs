using BusinessLogic.ViewModels;

namespace BusinessLogic.Interfaces
{
    /// <summary>
    /// Account service interface - Alias for ISystemAccountService
    /// Provides account management operations
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// Authenticate user by email and password
        /// </summary>
        Task<AccountViewModel?> AuthenticateAsync(string email, string password);

        /// <summary>
        /// Get all accounts
        /// </summary>
        Task<IEnumerable<AccountViewModel>> GetAllAccountsAsync();

        /// <summary>
        /// Get account by ID
        /// </summary>
        Task<AccountViewModel?> GetAccountByIdAsync(short id);

        /// <summary>
        /// Get account by email
        /// </summary>
        Task<AccountViewModel?> GetAccountByEmailAsync(string email);

        /// <summary>
        /// Create new account
        /// </summary>
        Task<bool> CreateAccountAsync(AccountViewModel model);

        /// <summary>
        /// Update existing account
        /// </summary>
        Task<bool> UpdateAccountAsync(AccountViewModel model);

        /// <summary>
        /// Delete account
        /// </summary>
        Task<bool> DeleteAccountAsync(short id);

        /// <summary>
        /// Search accounts by term and role
        /// </summary>
        Task<IEnumerable<AccountViewModel>> SearchAccountsAsync(string? searchTerm, int? role);

        /// <summary>
        /// Check if email already exists
        /// </summary>
        Task<bool> EmailExistsAsync(string email, short? excludeAccountId = null);

        /// <summary>
        /// Check if account can be deleted (no news articles)
        /// </summary>
        Task<bool> CanDeleteAccountAsync(short accountId);
    }
}