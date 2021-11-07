using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DistributedWarehouses.Domain.Repositories;
using DistributedWarehouses.Infrastructure.Models;
using InvoiceEntity = DistributedWarehouses.Domain.Entities.InvoiceEntity;
using InvoiceItemEntity = DistributedWarehouses.Domain.Entities.InvoiceItemEntity;
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
        public async Task<InvoiceEntity> AddInvoiceAsync(InvoiceEntity invoice)
        {
            var invoiceToAdd = _mapper.Map<Invoice>(invoice);

            await _distributedWarehousesContext.Invoices.Upsert(invoiceToAdd).On(i => i.Id)
                .WhenMatched(r => new Invoice { }).RunAsync(CancellationToken.None);
            return invoice;
        }

        public async Task<InvoiceItemEntity> AddInvoiceItemAsync(InvoiceItemEntity invoiceItem)
        {
            await _distributedWarehousesContext.InvoiceItems
                .Upsert(_mapper.Map<InvoiceItem>(invoiceItem))
                .On(ii => new { ii.Warehouse, ii.Invoice, ii.Item }).WhenMatched(ii => new InvoiceItem
                {
                    Quantity = ii.Quantity + invoiceItem.Quantity
                }).RunAsync(CancellationToken.None);

            return invoiceItem;
        }

        public async Task<bool> ExistsAsync<T>(T id)
        {
            return await _distributedWarehousesContext.Invoices.AnyAsync(i => i.Id.Equals(id) && !i.Reverted);
        }

        public async Task<int> RevertInvoice(Guid invoiceId)
        {
            var invoiceToRevert = await _distributedWarehousesContext.Invoices.Where(i => i.Id == invoiceId)
                .FirstOrDefaultAsync();
            invoiceToRevert.Reverted = true;
            _distributedWarehousesContext.Invoices.Update(invoiceToRevert);

            return await _distributedWarehousesContext.SaveChangesAsync();
        }

        public async Task Add<T>(T entity) where T : DistributableItemEntity
        {
            await AddInvoiceItemAsync(entity as InvoiceItemEntity);
        }
    }
}