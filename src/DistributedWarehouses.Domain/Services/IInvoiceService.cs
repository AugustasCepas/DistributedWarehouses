using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Dto;

namespace DistributedWarehouses.Domain.Services
{
    public interface IInvoiceService
    {
        IEnumerable<InvoiceEntity> GetInvoices();
        Task<InvoiceDto> GetInvoiceItemsAsync(Guid id);
        Task<InvoiceEntity> GetInvoiceAsync(Guid id);
        Task<int> AddInvoice(InvoiceEntity invoice);
        Task<int> RemoveInvoice(Guid id);
        IEnumerable<InvoiceItemEntity> GetInvoiceItems();
        InvoiceItemEntity GetInvoiceItem(string item, Guid warehouse, Guid invoice);
        Task<int> AddInvoiceItem(InvoiceItemEntity invoiceItem);
        public Task<int> RemoveInvoiceItem(string item, Guid warehouse, Guid invoice);
    }
}