using BusinessLogic.Interfaces;
using BusinessLogic.ViewModels;
using DataAccess.Context;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public class AccountService : IAccountService
    {
        private readonly FUNewsDbContext _ctx;

        public AccountService(FUNewsDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<List<AccountVM>> GetAllAsync()
        {
            return await _ctx.SystemAccounts
                .Select(a => new AccountVM
                {
                    AccountID = a.AccountID,
                    AccountName = a.AccountName,
                    AccountEmail = a.AccountEmail,
                    AccountRole = a.AccountRole,
                    AccountPassword = a.AccountPassword
                })
                .ToListAsync();
        }

        public async Task<AccountVM?> GetByIdAsync(int id)
        {
            return await _ctx.SystemAccounts
                .Where(a => a.AccountID == id)
                .Select(a => new AccountVM
                {
                    AccountID = a.AccountID,
                    AccountName = a.AccountName,
                    AccountEmail = a.AccountEmail,
                    AccountRole = a.AccountRole,
                    AccountPassword = a.AccountPassword
                })
                .FirstOrDefaultAsync();
        }

        public async Task<bool> CreateAsync(AccountVM model)
        {
            var acc = new SystemAccount
            {
                AccountName = model.AccountName,
                AccountEmail = model.AccountEmail,
                AccountRole = (short)model.AccountRole,
                AccountPassword = model.AccountPassword
            };

            _ctx.SystemAccounts.Add(acc);
            await _ctx.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(AccountVM model)
        {
            var acc = await _ctx.SystemAccounts.FindAsync(model.AccountID);
            if (acc == null) return false;

            acc.AccountName = model.AccountName;
            acc.AccountEmail = model.AccountEmail;
            acc.AccountRole = (short)model.AccountRole;
            acc.AccountPassword = model.AccountPassword;

            _ctx.SystemAccounts.Update(acc);
            await _ctx.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var acc = await _ctx.SystemAccounts.FindAsync(id);
            if (acc == null) return false;

            _ctx.SystemAccounts.Remove(acc);
            await _ctx.SaveChangesAsync();
            return true;
        }
    }
}
