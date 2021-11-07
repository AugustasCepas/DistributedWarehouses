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
        public Task<InvoiceEntity> AddInvoiceAsync(InvoiceEntity invoice);


        // Invoice Item
        Task<InvoiceItemEntity> AddInvoiceItemAsync(InvoiceItemEntity invoiceItem);
        Task<int> RevertInvoice(Guid invoiceId);
    }
}