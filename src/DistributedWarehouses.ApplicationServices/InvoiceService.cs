using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Domain.Repositories;
using DistributedWarehouses.Domain.RetrievalServices;
using DistributedWarehouses.Domain.Services;
using DistributedWarehouses.Domain.Validators;
using DistributedWarehouses.Dto;

namespace DistributedWarehouses.ApplicationServices
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRetrievalService _invoiceRetrievalService;
        private readonly IMappingService _mappingService;
        private readonly IValidator<Guid, IInvoiceRepository> _guidValidator;

        public InvoiceService(IInvoiceRetrievalService invoiceRetrievalService, IMappingService mappingService, IValidator<Guid, IInvoiceRepository> guidValidator)
        {
            _invoiceRetrievalService = invoiceRetrievalService;
            _mappingService = mappingService;
            _guidValidator = guidValidator;
        }

        public IEnumerable<InvoiceEntity> GetInvoices()
        {
            return _invoiceRetrievalService.GetInvoices();
        }

        public async Task<InvoiceDto> GetInvoiceItemsAsync(Guid id)
        {
            var invoice = _mappingService.Map<InvoiceDto>(await GetInvoice(id));
            var invoiceItems = _invoiceRetrievalService.GetInvoiceItems(id);
            invoice.Items = _mappingService.Map<IEnumerable<ItemInInvoiceInfoDto>>(invoiceItems);
            return invoice;
        }

        public async Task<InvoiceEntity> GetInvoice(Guid id)
        {
            await _guidValidator.ValidateAsync(id, false);
            return _invoiceRetrievalService.GetInvoice(id);
        }

        public Task<int> AddInvoice(InvoiceEntity invoice)
        {
            _guidValidator.ValidateAsync(invoice.Id, true);
            return _invoiceRetrievalService.AddInvoice(invoice);
        }

        public async Task<int> RemoveInvoice(Guid id)
        {
            await _guidValidator.ValidateAsync(id, false);
            return await _invoiceRetrievalService.RemoveInvoice(id);
        }

        public IEnumerable<InvoiceItemEntity> GetInvoiceItems()
        {
            return _invoiceRetrievalService.GetInvoiceItems();
        }

        public InvoiceItemEntity GetInvoiceItem(string item, Guid warehouse, Guid invoice)
        {
            return _invoiceRetrievalService.GetInvoiceItem(item, warehouse, invoice);
        }


        public Task<int> AddInvoiceItem(InvoiceItemEntity invoiceItem)
        {
            return _invoiceRetrievalService.AddInvoiceItem(invoiceItem);
        }

        public Task<int> RemoveInvoiceItem(string item, Guid warehouse, Guid invoice)
        {
            return _invoiceRetrievalService.RemoveInvoiceItem(item, warehouse, invoice);
        }
    }
}