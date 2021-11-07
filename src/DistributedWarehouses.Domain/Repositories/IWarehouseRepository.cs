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
        public Task<WarehouseInformation> GetWarehouseByItem(string sku, string property);


        // Warehouse Items
        WarehouseItemEntity GetWarehouseItem(string item, Guid warehouse);
        Task<WarehouseItemEntity> AddWarehouseItem(WarehouseItemEntity warehouseItemEntity);
        Task<int> UpdateWarehouseItemQuantity(string item, Guid warehouse, int quantity);
    }
}