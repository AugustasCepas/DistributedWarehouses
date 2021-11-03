using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Domain.Repositories;
using DistributedWarehouses.Domain.RetrievalServices;
using DistributedWarehouses.Dto;

namespace DistributedWarehouses.DomainServices
{
    public class InvoiceRetrievalService : IInvoiceRetrievalService
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public InvoiceRetrievalService(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public IEnumerable<InvoiceEntity> GetInvoices()
        {
            return _invoiceRepository.GetInvoices();
        }

        public InvoiceDto GetInvoiceItems(Guid invoiceGuid)
        {
            return _invoiceRepository.GetInvoiceItems(invoiceGuid);
        }

        public InvoiceEntity GetInvoice(Guid id)
        {
            return _invoiceRepository.GetInvoice(id);
        }

        public Task<int> AddInvoice(InvoiceEntity invoice)
        {
            return _invoiceRepository.AddInvoice(invoice);
        }

        public Task<int> RemoveInvoice(Guid id)
        {
            return _invoiceRepository.RemoveInvoice(id);
        }


        // Invoice Item

        public IEnumerable<InvoiceItemEntity> GetInvoiceItems()
        {
            return _invoiceRepository.GetInvoiceItems();
        }

        public InvoiceItemEntity GetInvoiceItem(string item, Guid warehouse, Guid invoice)
        {
            return _invoiceRepository.GetInvoiceItem(item, warehouse, invoice);
        }

        public Task<int> AddInvoiceItem(InvoiceItemEntity invoiceItem)
        {
            return _invoiceRepository.AddInvoiceItem(invoiceItem);
        }

        public Task<int> RemoveInvoiceItem(string item, Guid warehouse, Guid invoice)
        {
            return _invoiceRepository.RemoveInvoiceItem(item, warehouse, invoice);
        }
    }
}