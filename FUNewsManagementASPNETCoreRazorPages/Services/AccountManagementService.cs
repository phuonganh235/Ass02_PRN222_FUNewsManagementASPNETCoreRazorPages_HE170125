using BusinessObjects;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AccountManagementService : IAccountManagementService
    {
        private readonly IGenericRepository<SystemAccount> _accountRepository;
        private readonly IGenericRepository<NewsArticle> _newsRepository;
        private readonly ILogger<AccountManagementService> _logger;

        public AccountManagementService(
            IGenericRepository<SystemAccount> accountRepository,
            IGenericRepository<NewsArticle> newsRepository,
            ILogger<AccountManagementService> logger)
        {
            _accountRepository = accountRepository;
            _newsRepository = newsRepository;
            _logger = logger;
        }

        public async Task<(IEnumerable<SystemAccount> Accounts, int TotalCount)> SearchAccountsAsync(
            string? searchTerm, int? roleFilter, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var filters = new List<Expression<Func<SystemAccount, bool>>>();

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    string search = searchTerm.Trim().ToLower();
                    filters.Add(x =>
                        (x.AccountName != null && x.AccountName.ToLower().Contains(search)) ||
                        (x.AccountEmail != null && x.AccountEmail.ToLower().Contains(search)));
                }

                if (roleFilter.HasValue)
                    filters.Add(x => x.AccountRole == roleFilter.Value);

                var result = await _accountRepository.GetByPageAsync(
                    filters: filters,
                    orderBy: x => x.AccountName ?? string.Empty,
                    pageNumber: pageNumber,
                    pageSize: pageSize);

                return (result.Items, result.TotalCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching accounts");
                return (Enumerable.Empty<SystemAccount>(), 0);
            }
        }

        public async Task<SystemAccount?> GetAccountByIdAsync(short accountId)
        {
            var result = await _accountRepository.GetByPredicateAsync(x => x.AccountId == accountId);
            return result.FirstOrDefault();
        }

        public async Task<bool> CreateAccountAsync(SystemAccount account)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(account.AccountEmail)) return false;
                var email = account.AccountEmail.Trim().ToLower();

                if (!await IsEmailUniqueAsync(email))
                {
                    _logger.LogWarning("Duplicate email: {Email}", email);
                    return false;
                }

                account.AccountEmail = email;
                account.AccountPassword = HashPassword(account.AccountPassword?.Trim() ?? string.Empty);
                return await _accountRepository.AddAsync(account);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating account");
                return false;
            }
        }

        public async Task<bool> UpdateAccountAsync(SystemAccount account)
        {
            try
            {
                var email = account.AccountEmail.Trim().ToLower();
                if (!await IsEmailUniqueAsync(email, account.AccountId))
                    return false;

                account.AccountEmail = email;
                return await _accountRepository.UpdateAsync(account);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating account");
                return false;
            }
        }

        public async Task<bool> DeleteAccountAsync(short accountId)
        {
            if (!await CanDeleteAccountAsync(accountId))
                return false;
            return await _accountRepository.DeleteAsync(accountId);
        }

        public async Task<bool> ChangePasswordAsync(short accountId, string currentPassword, string newPassword)
        {
            var account = await GetAccountByIdAsync(accountId);
            if (account == null) return false;

            var hashedCurrent = HashPassword(currentPassword);
            if (account.AccountPassword != hashedCurrent) return false;

            account.AccountPassword = HashPassword(newPassword);
            return await _accountRepository.UpdateAsync(account);
        }

        public async Task<bool> IsEmailUniqueAsync(string email, short? excludeId = null)
        {
            var all = await _accountRepository.GetByPredicateAsync(x => x.AccountEmail.ToLower() == email.ToLower());
            if (excludeId.HasValue)
                return !all.Any(x => x.AccountId != excludeId.Value);
            return !all.Any();
        }

        public async Task<bool> CanDeleteAccountAsync(short accountId)
        {
            var used = await _newsRepository.GetByPredicateAsync(x => x.CreatedById == accountId);
            return !used.Any();
        }

        public async Task<List<RoleOption>> GetRoleOptionsAsync() => new()
        {
            new RoleOption { Value = 1, Text = "Staff" },
            new RoleOption { Value = 2, Text = "Lecturer" },
            new RoleOption { Value = 3, Text = "Admin" }
        };

        private string HashPassword(string password)
        {
            using var sha = System.Security.Cryptography.SHA256.Create();
            return Convert.ToBase64String(sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)));
        }
    }
}
