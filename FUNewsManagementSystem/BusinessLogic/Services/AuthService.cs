using BusinessLogic.Interfaces;
using DataAccess.Context;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public class AuthService : IAuthService
    {
        private readonly FUNewsDbContext _ctx;
        public AuthService(FUNewsDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<SystemAccount?> LoginAsync(string email, string password)
        {
            return await _ctx.SystemAccounts
                .FirstOrDefaultAsync(x => x.AccountEmail == email && x.AccountPassword == password);
        }
    }
}
