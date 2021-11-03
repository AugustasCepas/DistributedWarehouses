using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Dto;

namespace DistributedWarehouses.Domain.Services
{
    public interface IWarehouseService
    {
        IEnumerable<WarehouseEntity> GetWarehouses();
        WarehouseDto GetWarehouseInfo(Guid id);
        Task<int> AddWarehouse(WarehouseEntity warehouseEntity);
        Task<int> RemoveWarehouse(Guid id);


        // Warehouse Item
        IEnumerable<WarehouseItemEntity> GetWarehouseItems();
        WarehouseItemEntity GetWarehouseItem(string item, Guid warehouse);
        Task<int> AddWarehouseItem(WarehouseItemEntity warehouseItemEntity);
        Task<int> RemoveWarehouseItem(string item, Guid warehouse);
        Task<int> SellWarehouseItem(ItemSellDto dto);

    }
}