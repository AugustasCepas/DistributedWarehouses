using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Domain.RetrievalServices;
using DistributedWarehouses.Domain.Services;
using DistributedWarehouses.Dto;

namespace DistributedWarehouses.ApplicationServices
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRetrievalService _invoiceRetrievalRepository;

        public InvoiceService(IInvoiceRetrievalService invoiceRetrievalRepository)
        {
            _invoiceRetrievalRepository = invoiceRetrievalRepository;
        }

        public IEnumerable<InvoiceEntity> GetInvoices()
        {
            return _invoiceRetrievalRepository.GetInvoices();
        }

        public InvoiceDto GetInvoiceItems(Guid id)
        {
            return _invoiceRetrievalRepository.GetInvoiceItems(id);
        }

        public InvoiceEntity GetInvoice(Guid id)
        {
            return _invoiceRetrievalRepository.GetInvoice(id);
        }

        public Task<int> AddInvoice(InvoiceEntity invoice)
        {
            return _invoiceRetrievalRepository.AddInvoice(invoice);
        }

        public Task<int> RemoveInvoice(Guid id)
        {
            return _invoiceRetrievalRepository.RemoveInvoice(id);
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