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


        // Warehouse Item
        Task<WarehouseItemEntity> AddWarehouseItem(WarehouseItemEntity warehouseItemEntity);

    }
}