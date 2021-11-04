using System;
using DistributedWarehouses.ApplicationServices;
using DistributedWarehouses.ApplicationServices.Validators;
using DistributedWarehouses.Domain;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Domain.Repositories;
using DistributedWarehouses.Domain.RetrievalServices;
using DistributedWarehouses.Domain.Services;
using DistributedWarehouses.DomainServices;
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
            services.AddScoped<IRepositoryProvider, RepositoryProvider>();
        }

        private static void AddRetrievalServices(this IServiceCollection services)
        {
            services.AddScoped<IWarehouseRetrievalService, WarehouseRetrievalService>();
            services.AddScoped<IReservationRetrievalService, ReservationRetrievalService>();
            services.AddScoped<IInvoiceRetrievalService, InvoiceRetrievalService>();
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


            services.AddScoped<IValidator<InvoiceEntity>, InvoiceValidator>();
            services.AddScoped<Domain.Validators.IValidator<InvoiceEntity, IInvoiceRepository>>(provider =>
                new Validator<InvoiceEntity, IInvoiceRepository>(provider.GetRequiredService<IValidator<InvoiceEntity>>(), provider.GetRequiredService<IRepositoryProvider>()));

            services.AddScoped<IValidator<ItemEntity>, ItemValidator>();
            services.AddScoped<Domain.Validators.IValidator<ItemEntity, IItemRepository>>(provider =>
                new Validator<ItemEntity, IItemRepository>(provider.GetRequiredService<IValidator<ItemEntity>>(), provider.GetRequiredService<IRepositoryProvider>()));
        }
    }
}