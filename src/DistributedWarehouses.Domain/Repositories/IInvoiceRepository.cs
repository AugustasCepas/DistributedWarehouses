using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace DistributedWarehouses.Domain.Repositories
{
    public interface IInvoiceRepository : IRepository
    {
        IDbContextTransaction GetTransaction();
        IEnumerable<InvoiceEntity> GetInvoices();
        Task<InvoiceEntity> GetInvoiceAsync(Guid invoiceGuid);
        IEnumerable<InvoiceItemEntity> GetInvoiceItems(Guid invoiceGuid);
        Task<int> AddInvoice(InvoiceEntity invoice);
        Task<int> RemoveInvoice(Guid id);
        public Task<InvoiceEntity> AddInvoiceAsync(InvoiceEntity invoice);


        // Invoice Item
        IEnumerable<InvoiceItemEntity> GetInvoiceItems();
        InvoiceItemEntity GetInvoiceItem(string item, Guid warehouse, Guid invoice);
        Task<InvoiceItemEntity> AddInvoiceItemAsync(InvoiceItemEntity invoiceItem);
        Task<int> RemoveInvoiceItem(string item, Guid warehouse, Guid invoice);
        Task<int> RemoveInvoiceItems(IEnumerable<InvoiceItemEntity> invoiceItems);
        IEnumerable<WarehouseItemEntity> GetWarehouseItemsFromInvoice(Guid invoiceGuid);
        Task<int> RevertInvoice(Guid invoiceId);
    }
}