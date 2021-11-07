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
        Task<WarehouseDto> GetWarehouseInfo(Guid id);
        Task<WarehouseItemEntity> AddWarehouseItem(Guid warehouse, string sku, int quantity);
    }
}