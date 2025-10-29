using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IAccountManagementService
    {
        Task<(IEnumerable<SystemAccount> Accounts, int TotalCount)> SearchAccountsAsync(
            string? searchTerm, int? roleFilter, int pageNumber = 1, int pageSize = 10);
        Task<SystemAccount?> GetAccountByIdAsync(short accountId);
        Task<bool> CreateAccountAsync(SystemAccount account);
        Task<bool> UpdateAccountAsync(SystemAccount account);
        Task<bool> DeleteAccountAsync(short accountId);
        Task<bool> ChangePasswordAsync(short accountId, string currentPassword, string newPassword);
        Task<bool> IsEmailUniqueAsync(string email, short? excludeAccountId = null);
        Task<bool> CanDeleteAccountAsync(short accountId);
        Task<List<RoleOption>> GetRoleOptionsAsync();
    }
}
