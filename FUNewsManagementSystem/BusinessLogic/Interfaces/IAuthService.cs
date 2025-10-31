﻿using DataAccess.Entities;

namespace BusinessLogic.Interfaces
{
    public interface IAuthService
    {
        Task<SystemAccount?> LoginAsync(string email, string password);
        bool IsAdmin(SystemAccount acc);
        bool IsStaff(SystemAccount acc);
    }
}
