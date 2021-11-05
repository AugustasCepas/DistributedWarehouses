using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Dto;

namespace DistributedWarehouses.Domain.Repositories
{
    public interface IWarehouseRepository : IRepository
    {
        public IEnumerable<WarehouseEntity> GetWarehouses();
        public WarehouseDto GetWarehouseInfo(Guid id);
        public WarehouseEntity GetWarehouse(Guid id);
        public Task<int> AddWarehouse(WarehouseEntity warehouseEntity);
        public Task<int> RemoveWarehouse(Guid id);


        // Warehouse Items
        IEnumerable<WarehouseItemEntity> GetWarehouseItems(Guid id);
        WarehouseItemEntity GetWarehouseItem(string item, Guid warehouse);
        Task<int> AddWarehouseItem(WarehouseItemEntity warehouseItemEntity);
        Task<int> RemoveWarehouseItem(string item, Guid warehouse);
        Task<int> UpdateWarehouseItemQuantity(string item, Guid warehouse, int quantity);
        Task<WarehouseItemEntity> GetWarehouseByFreeSpace();
        Task<int> AddInvoiceItemsToWarehouseAsync(InvoiceItemEntity invoiceItem);
        public Task<WarehouseItemEntity> GetLargestWarehouseByFreeItemsQuantityAsync(string sku);
    }
}