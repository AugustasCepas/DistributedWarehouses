using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DistributedWarehouses.Domain;
using DistributedWarehouses.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DistributedWarehouses.Api.Injections
{
    public static class StartupInjection
    {
        public static void AddInjections(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRepositories();
        }

        private static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IItemsRepository, ItemsRepository>();
        }
    }
}
