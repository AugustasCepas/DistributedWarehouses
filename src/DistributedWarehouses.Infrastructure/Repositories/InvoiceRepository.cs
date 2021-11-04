using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DistributedWarehouses.Domain.Repositories;
using DistributedWarehouses.Dto;
using DistributedWarehouses.Infrastructure.Models;
using InvoiceEntity = DistributedWarehouses.Domain.Entities.InvoiceEntity;
using InvoiceModel = DistributedWarehouses.Infrastructure.Models.Invoice;
using InvoiceItemEntity = DistributedWarehouses.Domain.Entities.InvoiceItemEntity;
using InvoiceItemModel = DistributedWarehouses.Infrastructure.Models.InvoiceItem;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace DistributedWarehouses.Infrastructure.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly DistributedWarehousesContext _distributedWarehousesContext;
        private readonly IMapper _mapper;

        public InvoiceRepository(DistributedWarehousesContext distributedWarehousesContext, IMapper mapper)
        {
            _distributedWarehousesContext = distributedWarehousesContext;
            _mapper = mapper;
        }

        public IEnumerable<InvoiceEntity> GetInvoices()
        {
            return _mapper.ProjectTo<InvoiceEntity>(_distributedWarehousesContext.Invoices).AsEnumerable();
        }

        public Task<InvoiceEntity> GetInvoice(Guid invoiceGuid)
        {
            return _mapper.ProjectTo<InvoiceEntity>(_distributedWarehousesContext.Invoices)
                .FirstOrDefaultAsync(i => i.Id == invoiceGuid);
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
                .Select(t =>
                    new Tuple<string, int, Guid>(t.invoiceItem.Item, t.invoiceItem.Quantity, t.invoiceItem.Warehouse));

            return _mapper.ProjectTo<InvoiceItemEntity>(query).AsEnumerable();
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

        public Task<bool> ExistsAsync<T>(T id)
        {
            return _distributedWarehousesContext.Invoices.AnyAsync(i => i.Id.Equals(id));
        }
    }
}