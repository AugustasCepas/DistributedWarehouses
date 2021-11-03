using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Repositories;
using DistributedWarehouses.Dto;
using DistributedWarehouses.Infrastructure.Models;
using InvoiceEntity = DistributedWarehouses.Domain.Entities.InvoiceEntity;
using InvoiceModel = DistributedWarehouses.Infrastructure.Models.Invoice;
using InvoiceItemEntity = DistributedWarehouses.Domain.Entities.InvoiceItemEntity;
using InvoiceItemModel = DistributedWarehouses.Infrastructure.Models.InvoiceItem;
using AutoMapper.QueryableExtensions;

namespace DistributedWarehouses.Infrastructure.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly DistributedWarehousesContext _distributedWarehousesContext;

        public InvoiceRepository(DistributedWarehousesContext distributedWarehousesContext)
        {
            _distributedWarehousesContext = distributedWarehousesContext;
        }

        public IEnumerable<InvoiceEntity> GetInvoices()
        {
            return _distributedWarehousesContext.Invoices.Select(i => new InvoiceEntity
            {
                Id = i.Id,
                CreatedAt = i.CreatedAt
            }).AsEnumerable();
        }

        public InvoiceEntity GetInvoice(Guid invoiceGuid)
        {
            return _distributedWarehousesContext.Invoices
                .Where(i => i.Id == invoiceGuid)
                .Select(i => new InvoiceEntity
                {
                    Id = i.Id,
                    CreatedAt = i.CreatedAt
                }).FirstOrDefault();
        }

        /// <summary>
        /// Get invoice items
        /// </summary>
        /// <param name="invoiceGuid"></param>
        /// <returns></returns>
        public IEnumerable<InvoiceItemEntity> GetInvoiceItems(Guid invoiceGuid)
        {
            var invoices = _distributedWarehousesContext.Invoices;
            var invoiceItems = _distributedWarehousesContext.InvoiceItems;

            var query = invoices
                .GroupJoin(invoiceItems, invoice => invoice.Id, invoiceItem => invoiceItem.Invoice,
                    (invoice, invoiceItemGroup) => new {invoice, invoiceItemGroup})
                .SelectMany(t => t.invoiceItemGroup.DefaultIfEmpty(), (t, invoiceItem) => new {t, invoiceItem})
                .Where(t => t.invoiceItem.Invoice == invoiceGuid)
                .ProjectTo<InvoiceItemEntity>()
                .Select(t => new ItemInInvoiceInfoDto
                {
                    ItemId = t.invoiceItem.Item,
                    PurchasedQuantity = t.invoiceItem.Quantity,
                    WarehouseId = t.invoiceItem.Warehouse
                });

            return query.AsEnumerable();
        }

        public Task<int> AddInvoice(InvoiceEntity invoice)
        {
            _distributedWarehousesContext.Invoices.Add(new InvoiceModel
            {
                Id = invoice.Id,
                CreatedAt = invoice.CreatedAt
            });
            return _distributedWarehousesContext.SaveChangesAsync();
        }

        public async Task<int> RemoveInvoice(Guid id)
        {
            _distributedWarehousesContext.Invoices.Remove(
                await _distributedWarehousesContext.FindAsync<InvoiceModel>(id));
            return await _distributedWarehousesContext.SaveChangesAsync();
        }


        // Invoice Item
        public IEnumerable<InvoiceItemEntity> GetInvoiceItems()
        {
            return _distributedWarehousesContext.InvoiceItems.Select(i => new InvoiceItemEntity
            {
                Item = i.Item,
                Quantity = i.Quantity,
                Warehouse = i.Warehouse,
                Invoice = i.Invoice
            }).AsEnumerable();
        }

        public InvoiceItemEntity GetInvoiceItem(string item, Guid warehouse, Guid invoice)
        {
            return _distributedWarehousesContext.InvoiceItems
                .Where(i => i.Item == item && i.Warehouse == warehouse && i.Invoice == invoice)
                .Select(i => new InvoiceItemEntity
                {
                    Item = i.Item,
                    Quantity = i.Quantity
                }).FirstOrDefault();
        }

        public Task<int> AddInvoiceItem(InvoiceItemEntity invoiceItem)
        {
            _distributedWarehousesContext.InvoiceItems.Add(new InvoiceItemModel
            {
                Item = invoiceItem.Item,
                Quantity = invoiceItem.Quantity,
                Warehouse = invoiceItem.Warehouse,
                Invoice = invoiceItem.Invoice
            });
            return _distributedWarehousesContext.SaveChangesAsync();
        }

        public async Task<int> RemoveInvoiceItem(string item, Guid warehouse, Guid invoice)
        {
            _distributedWarehousesContext.InvoiceItems.Remove(
                await _distributedWarehousesContext.FindAsync<InvoiceItemModel>(item, warehouse, invoice));
            return await _distributedWarehousesContext.SaveChangesAsync();
        }
    }
}