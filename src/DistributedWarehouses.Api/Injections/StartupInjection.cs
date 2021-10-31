using DistributedWarehouses.ApplicationServices;
using DistributedWarehouses.Domain;
using DistributedWarehouses.Domain.Repositories;
using DistributedWarehouses.Domain.RetrievalServices;
using DistributedWarehouses.Domain.Services;
using DistributedWarehouses.DomainServices;
using DistributedWarehouses.DomainServices.Validators;
using DistributedWarehouses.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DistributedWarehouses.Api.Injections
{
    public static class StartupInjection
    {
        public static void AddInjections(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddServices();
            services.AddRetrievalServices();
            services.AddRepositories();
            services.AddValidators();
        }

        private static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<IWarehouseRepository, WarehouseRepository>();
            services.AddScoped<IReservationRepository, ReservationRepository>();
            services.AddScoped<IInvoiceRepository, InvoiceRepository>();
        }

        private static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<IWarehouseService, WarehouseService>();
            services.AddScoped<IReservationService, ReservationService>();
            services.AddScoped<IInvoiceService, InvoiceService>();
        }

        private static void AddRetrievalServices(this IServiceCollection services)
        {
            services.AddScoped<IItemRetrievalService, ItemRetrievalService>();
            services.AddScoped<IWarehouseRetrievalService, WarehouseRetrievalService>();
            services.AddScoped<IReservationRetrievalService, ReservationRetrievalService>();
            services.AddScoped<IInvoiceRetrievalService, InvoiceRetrievalService>();
        }

        private static void AddValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<string>, SkuValidator>();
        }
    }
}