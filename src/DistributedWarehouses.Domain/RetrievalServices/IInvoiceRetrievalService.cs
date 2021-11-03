using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Dto;

namespace DistributedWarehouses.Domain.RetrievalServices
{
    public interface IInvoiceRetrievalService
    {
        IEnumerable<InvoiceEntity> GetInvoices();
        IEnumerable<InvoiceItemEntity> GetInvoiceItems(Guid invoiceGuid);
        InvoiceEntity GetInvoice(Guid id);
        Task<int> AddInvoice(InvoiceEntity invoice);
        Task<int> RemoveInvoice(Guid id);


        // Invoice Item
        IEnumerable<InvoiceItemEntity> GetInvoiceItems();
        InvoiceItemEntity GetInvoiceItem(string item, Guid warehouse, Guid invoice);
        Task<int> AddInvoiceItem(InvoiceItemEntity invoiceItem);
        Task<int> RemoveInvoiceItem(string item, Guid warehouse, Guid invoice);
    }
}