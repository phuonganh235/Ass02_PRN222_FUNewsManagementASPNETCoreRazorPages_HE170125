using BusinessLogic.Interfaces;
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

        public async Task<SystemAccount?> GetAsync(short accountId)
        {
            return await _ctx.SystemAccounts.FirstOrDefaultAsync(x => x.AccountID == accountId);
        }

        public async Task<List<SystemAccount>> GetAllAsync()
        {
            return await _ctx.SystemAccounts.OrderBy(x => x.AccountName).ToListAsync();
        }

        public async Task<SystemAccount> CreateAsync(string name, string email, int role, string password)
        {
            var acc = new SystemAccount
            {
                AccountName = name,
                AccountEmail = email,
                AccountRole = role,
                AccountPassword = password
            };
            _ctx.SystemAccounts.Add(acc);
            await _ctx.SaveChangesAsync();
            return acc;
        }
    }
}
