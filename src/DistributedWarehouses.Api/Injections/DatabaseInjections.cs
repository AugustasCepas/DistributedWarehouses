using DistributedWarehouses.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DistributedWarehouses.Api.Injections
{
    public static class DatabaseInjections
    {
        public static void AddDatabases(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetValue<string>("Database:ConnectionString");
            var databasePassword = configuration.GetValue<string>("Database:Password");
            connectionString = connectionString.Replace("myPassword", databasePassword);
            services.AddDbContext<DistributedWarehousesContext>(options => { options.UseSqlServer(connectionString);
                options.EnableSensitiveDataLogging();
            });
        }
    }
}