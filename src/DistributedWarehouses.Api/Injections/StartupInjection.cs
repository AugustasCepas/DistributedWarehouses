using DistributedWarehouses.ApplicationServices;
using DistributedWarehouses.Domain;
using DistributedWarehouses.Domain.Repositories;
using DistributedWarehouses.Domain.Services;
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
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<IItemRetrievalService, ItemRetrievalService>();

            services.AddScoped<IWarehouseRepository, WarehouseRepository>();
            services.AddScoped<IWarehouseService, WarehouseService>();
            services.AddScoped<IWarehouseRetrievalService, WarehouseRetrievalService>();

            services.AddScoped<IReservationRepository, ReservationRepository>();
            services.AddScoped<IReservationService, ReservationService>();
            services.AddScoped<IReservationRetrievalService, ReservationRetrievalService>();

            services.AddScoped<IWarehouseItemRepository, WarehouseItemRepository>();
        }
    }
}