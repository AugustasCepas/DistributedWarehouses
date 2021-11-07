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
        public Task<WarehouseDto> GetWarehouseInfo(Guid id);
        public Task<WarehouseEntity> GetWarehouse(Guid id);
        public Task<WarehouseInformation> GetWarehouseByItem(string sku, string property);
        Task<WarehouseItemEntity> GetWarehouseItem(string item, Guid warehouse);
        Task<WarehouseItemEntity> AddWarehouseItem(WarehouseItemEntity warehouseItemEntity);
        Task<int> UpdateWarehouseItemQuantity(string item, Guid warehouse, int quantity);
    }
}