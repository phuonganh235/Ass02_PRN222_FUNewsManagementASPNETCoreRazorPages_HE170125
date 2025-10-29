using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public static class RepositoryServiceCollectionExtensions
    {
        public static IServiceCollection AddGenericRepository<TContext>(this IServiceCollection services)
            where TContext : DbContext
        {
            // Đăng ký repository tổng quát
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            return services;
        }
    }
}
