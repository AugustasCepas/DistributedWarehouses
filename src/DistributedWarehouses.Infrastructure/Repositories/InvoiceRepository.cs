﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DistributedWarehouses.Domain.Repositories;
using DistributedWarehouses.Dto;
using DistributedWarehouses.Infrastructure.Models;
using InvoiceEntity = DistributedWarehouses.Domain.Entities.InvoiceEntity;
using InvoiceItemEntity = DistributedWarehouses.Domain.Entities.InvoiceItemEntity;
using AutoMapper.QueryableExtensions;
using DistributedWarehouses.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

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

        public IDbContextTransaction GetTransaction()
        {
            return _distributedWarehousesContext.Database.BeginTransaction();
        }

        public IEnumerable<InvoiceEntity> GetInvoices()
        {
            return _mapper.ProjectTo<InvoiceEntity>(_distributedWarehousesContext.Invoices).AsEnumerable();
        }

        public Task<InvoiceEntity> GetInvoiceAsync(Guid invoiceGuid)
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
                    // new Tuple<string, int, Guid, Guid>(t.invoiceItem.Item, t.invoiceItem.Quantity, t.invoiceItem.Warehouse, invoiceGuid));
                    new InvoiceItem
                    {
                        Item = t.invoiceItem.Item,
                        Quantity = t.invoiceItem.Quantity,
                        Warehouse = t.invoiceItem.Warehouse,
                        Invoice = invoiceGuid
                    });


            return _mapper.ProjectTo<InvoiceItemEntity>(query).AsEnumerable();
        }

        public IEnumerable<WarehouseItemEntity> GetWarehouseItemsFromInvoice(Guid invoiceGuid)
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

            return _mapper.ProjectTo<WarehouseItemEntity>(query).AsEnumerable();
        }

        public Task<int> AddInvoice(InvoiceEntity invoice)
        {
            _distributedWarehousesContext.Invoices.Add(new Invoice
            {
                Id = invoice.Id,
                CreatedAt = invoice.CreatedAt
            });
            return _distributedWarehousesContext.SaveChangesAsync();
        }

        public async Task<int> RemoveInvoice(Guid id)
        {
            _distributedWarehousesContext.Invoices.Remove(
                await _distributedWarehousesContext.FindAsync<Invoice>(id));
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
            _distributedWarehousesContext.InvoiceItems.Add(new InvoiceItem
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
                await _distributedWarehousesContext.FindAsync<InvoiceItem>(item, warehouse, invoice));
            return await _distributedWarehousesContext.SaveChangesAsync();
        }

        public async Task<int> RemoveInvoiceItems(IEnumerable<InvoiceItemEntity> invoiceItems)
        {
            var toRemove = _mapper.Map<List<InvoiceItem>>(invoiceItems);
            _distributedWarehousesContext.InvoiceItems.RemoveRange(toRemove);
            return await _distributedWarehousesContext.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync<T>(T id)
        {
            return await _distributedWarehousesContext.Invoices.AnyAsync(i => i.Id.Equals(id));
        }

        public async Task<int> RevertInvoice(Guid invoiceId)
        {
            var invoiceToRevert = await _distributedWarehousesContext.Invoices.Where(i => i.Id == invoiceId)
                .FirstOrDefaultAsync();
            invoiceToRevert.Reverted = true;
            _distributedWarehousesContext.Invoices.Update(invoiceToRevert);

            return await _distributedWarehousesContext.SaveChangesAsync();
        }
    }
}