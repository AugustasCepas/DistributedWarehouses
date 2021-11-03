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
        private readonly IInvoiceRetrievalService _invoiceRetrievalRepository;
        private readonly IMappingService _mappingService;
        private readonly IValidator<(bool, Guid), IInvoiceRepository> _guidValidator;

        public InvoiceService(IInvoiceRetrievalService invoiceRetrievalRepository, IMappingService mappingService, IValidator<
            (bool, Guid), IInvoiceRepository> guidValidator)
        {
            _invoiceRetrievalRepository = invoiceRetrievalRepository;
            _mappingService = mappingService;
            _guidValidator = guidValidator;
        }

        public IEnumerable<InvoiceEntity> GetInvoices()
        {
            return _invoiceRetrievalRepository.GetInvoices();
        }

        public async Task<InvoiceDto> GetInvoiceItemsAsync(Guid id)
        {
            var invoice = _mappingService.Map<InvoiceDto>(await GetInvoice(id));
            var invoiceItems = _invoiceRetrievalRepository.GetInvoiceItems(id);
            invoice.Items = _mappingService.Map<IEnumerable<ItemInInvoiceInfoDto>>(invoiceItems);
            return invoice;
        }

        public async Task<InvoiceEntity> GetInvoice(Guid id)
        {
            await _guidValidator.ValidateAsync((false, id));
            return _invoiceRetrievalRepository.GetInvoice(id);
        }

        public Task<int> AddInvoice(InvoiceEntity invoice)
        {
            return _invoiceRetrievalRepository.AddInvoice(invoice);
        }

        public async Task<int> RemoveInvoice(Guid id)
        {
            await _guidValidator.ValidateAsync((false, id));
            return await _invoiceRetrievalRepository.RemoveInvoice(id);
        }

        public IEnumerable<InvoiceItemEntity> GetInvoiceItems()
        {
            return _invoiceRetrievalRepository.GetInvoiceItems();
        }

        public InvoiceItemEntity GetInvoiceItem(string item, Guid warehouse, Guid invoice)
        {
            return _invoiceRetrievalRepository.GetInvoiceItem(item, warehouse, invoice);
        }


        public Task<int> AddInvoiceItem(InvoiceItemEntity invoiceItem)
        {
            return _invoiceRetrievalRepository.AddInvoiceItem(invoiceItem);
        }

        public Task<int> RemoveInvoiceItem(string item, Guid warehouse, Guid invoice)
        {
            return _invoiceRetrievalRepository.RemoveInvoiceItem(item, warehouse, invoice);
        }
    }
}