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
        }

        private static void AddRetrievalServices(this IServiceCollection services)
        {
            services.AddScoped<IWarehouseRetrievalService, WarehouseRetrievalService>();
            services.AddScoped<IReservationRetrievalService, ReservationRetrievalService>();
            services.AddScoped<IInvoiceRetrievalService, InvoiceRetrievalService>();
        }

        private static void AddValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<(bool, string)>, SkuValidator>();
            services.AddScoped<Domain.Validators.IValidator<(bool, string), IItemRepository>>(provider =>
                new Validator<(bool, string), IItemRepository>(provider.GetRequiredService<IValidator<(bool, string)>>()));

            services.AddScoped<IValidator<(bool, Guid)>, GuidValidator<IInvoiceRepository>>(provider =>
                new GuidValidator<IInvoiceRepository>(provider.GetRequiredService<IInvoiceRepository>()));
            services.AddScoped<IValidator<(bool, Guid)>, GuidValidator<IReservationRepository>>(provider =>
                new GuidValidator<IReservationRepository>(provider.GetRequiredService<IReservationRepository>()));
            services.AddScoped<IValidator<(bool, Guid)>, GuidValidator<IWarehouseRepository>>(provider =>
                new GuidValidator<IWarehouseRepository>(
                    provider.GetRequiredService<IWarehouseRepository>()));

            services.AddScoped<Domain.Validators.IValidator<(bool, Guid), IInvoiceRepository>>(provider =>
                new Validator<(bool, Guid), IInvoiceRepository>(provider.GetRequiredService<IValidator<(bool, Guid)>>()));
            services.AddScoped<Domain.Validators.IValidator<(bool, Guid), IReservationRepository>>(provider =>
                new Validator<(bool, Guid), IReservationRepository>(provider.GetRequiredService<IValidator<(bool, Guid)>>()));
            services.AddScoped<Domain.Validators.IValidator<(bool, Guid), IWarehouseRepository>>(provider =>
                new Validator<(bool, Guid), IWarehouseRepository>(provider.GetRequiredService<IValidator<(bool, Guid)>>()));


            services.AddScoped<IValidator<InvoiceEntity>, InvoiceValidator>();
            services.AddScoped<Domain.Validators.IValidator<InvoiceEntity, IRepository>>(provider =>
                new Validator<InvoiceEntity, IRepository>(provider.GetRequiredService<IValidator<InvoiceEntity>>()));

            services.AddScoped<IValidator<ItemEntity>, ItemValidator>();
            services.AddScoped<Domain.Validators.IValidator<ItemEntity, IItemRepository>>(provider =>
                new Validator<ItemEntity, IItemRepository>(provider.GetRequiredService<IValidator<ItemEntity>>()));
        }
    }
}