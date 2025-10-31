using BusinessLogic.Interfaces;
using DataAccess.Context;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BusinessLogic.Services
{
    public class AuthService : IAuthService
    {
        private readonly FUNewsDbContext _ctx;
        private readonly IConfiguration _config;

        public AuthService(FUNewsDbContext ctx, IConfiguration config)
        {
            _ctx = ctx;
            _config = config;
        }

        public async Task<SystemAccount?> LoginAsync(string email, string password)
        {
            // Try DB first
            var user = await _ctx.SystemAccounts
                .FirstOrDefaultAsync(a =>
                    a.AccountEmail == email &&
                    a.AccountPassword == password);

            // If not found -> fallback to Admin in appsettings.json
            if (user == null)
            {
                var adminEmail = _config["AdminAccount:Email"];
                var adminPass = _config["AdminAccount:Password"];

                if (email == adminEmail && password == adminPass)
                {
                    user = new SystemAccount
                    {
                        AccountID = 0,
                        AccountName = "Default Admin",
                        AccountEmail = adminEmail,
                        AccountRole = 3,
                        AccountPassword = adminPass
                    };
                }
            }

            return user;
        }

        public bool IsAdmin(SystemAccount acc) => acc.AccountRole == 3;
        public bool IsStaff(SystemAccount acc) => acc.AccountRole == 2;
    }
}
