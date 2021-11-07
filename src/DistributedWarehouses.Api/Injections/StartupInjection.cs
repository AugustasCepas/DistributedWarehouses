using System;
using DistributedWarehouses.ApplicationServices;
using DistributedWarehouses.ApplicationServices.Validators;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Domain.Repositories;
using DistributedWarehouses.Domain.Services;
using DistributedWarehouses.Dto;
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
            services.AddScoped<IRepositoryProvider, RepositoryProvider>();
        }
        
        private static void AddValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<string>, SkuValidator>();
            services.AddScoped<Domain.Validators.IValidator<string, IItemRepository>>(provider =>
                new Validator<string, IItemRepository>(provider.GetRequiredService<IValidator<string>>(), provider.GetRequiredService<IRepositoryProvider>()));

            services.AddScoped<IValidator<Guid>, GuidValidator>();

            services.AddScoped<Domain.Validators.IValidator<Guid, IInvoiceRepository>, Validator<Guid, IInvoiceRepository>>();
            services.AddScoped<Domain.Validators.IValidator<Guid, IReservationRepository>, Validator<Guid, IReservationRepository>>();
            services.AddScoped<Domain.Validators.IValidator<Guid, IWarehouseRepository>, Validator<Guid, IWarehouseRepository>>();


            services.AddScoped<IValidator<ItemSellDto>, ItemSellValidator>();
            services.AddScoped<Domain.Validators.IValidator<ItemSellDto, IInvoiceRepository>>(provider =>
                new Validator<ItemSellDto, IInvoiceRepository>(provider.GetRequiredService<IValidator<ItemSellDto>>(), provider.GetRequiredService<IRepositoryProvider>()));

            services.AddScoped<IValidator<ItemEntity>, ItemValidator>();
            services.AddScoped<Domain.Validators.IValidator<ItemEntity, IItemRepository>>(provider =>
                new Validator<ItemEntity, IItemRepository>(provider.GetRequiredService<IValidator<ItemEntity>>(), provider.GetRequiredService<IRepositoryProvider>()));
        }
    }
}