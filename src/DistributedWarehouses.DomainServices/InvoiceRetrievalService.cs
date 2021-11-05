using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Domain.Repositories;
using DistributedWarehouses.Domain.RetrievalServices;
using DistributedWarehouses.Dto;
using DistributedWarehouses.Infrastructure.Models;

namespace DistributedWarehouses.DomainServices
{
    public class InvoiceRetrievalService : IInvoiceRetrievalService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IWarehouseRepository _warehouseRepository;

        public InvoiceRetrievalService(IInvoiceRepository invoiceRepository, IWarehouseRepository warehouseRepository)
        {
            _invoiceRepository = invoiceRepository;
            _warehouseRepository = warehouseRepository;
        }

        public IEnumerable<InvoiceEntity> GetInvoices()
        {
            return _invoiceRepository.GetInvoices();
        }

        public IEnumerable<InvoiceItemEntity> GetInvoiceItems(Guid invoiceGuid)
        {
            return _invoiceRepository.GetInvoiceItems(invoiceGuid);
        }

        public async Task<InvoiceEntity> GetInvoice(Guid id)
        {
            return await _invoiceRepository.GetInvoiceAsync(id);
        }

        public Task<int> AddInvoice(InvoiceEntity invoice)
        {
            return _invoiceRepository.AddInvoice(invoice);
        }

        public Task<int> RemoveInvoice(Guid id)
        {
            return _invoiceRepository.RemoveInvoice(id);
        }

        public Task<int> RevertInvoice(Guid id)
        {
            return _invoiceRepository.RevertInvoice(id);
        }

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

        public Task<int> RemoveInvoiceItems(List<InvoiceItemEntity> invoiceItems)
        {
            return _invoiceRepository.RemoveInvoiceItems(invoiceItems);
        }

        /// <summary>
        /// Return Goods From Invoice
        /// Get Invoice Items
        /// Get Warehouse To Return Items
        /// Return Items To Warehouse
        /// Remove Invoice Items
        /// Remove Invoice
        /// </summary>
        /// <param name="id">Warehouse guid</param>
        /// <returns></returns>
        public async Task<int> ReturnInvoice(Guid id)
        {
            using (var transaction = _invoiceRepository.GetTransaction())
            {
                try
                {
                    var invoiceItems = _invoiceRepository.GetInvoiceItems(id).ToList();

                    await ReturnItemsToWarehouses(invoiceItems);
                    await RemoveInvoiceItems(invoiceItems);
                    await RevertInvoice(id);
                    await transaction.CommitAsync();

                    return invoiceItems.Count;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        private async Task<int> ReturnItemsToWarehouses(List<InvoiceItemEntity> invoiceItems)
        {
            var differentItemsReturned = 0;

            foreach (var item in invoiceItems)
            {
                var itemsToReturn = item.Quantity;
                while (itemsToReturn > 0)
                {
                    var emptiestWarehouse = await _warehouseRepository.GetWarehouseByFreeSpace();
                    var warehouseFreeSpace = emptiestWarehouse.Quantity;
                    item.Warehouse = emptiestWarehouse.Warehouse;
                    var itemsToWarehouse =
                        warehouseFreeSpace > itemsToReturn ? itemsToReturn : warehouseFreeSpace;
                    await _warehouseRepository.AddInvoiceItemsToWarehouseAsync(item);
                    itemsToReturn -= itemsToWarehouse;
                }

                differentItemsReturned++;
            }

            return differentItemsReturned;
        }
    }
}