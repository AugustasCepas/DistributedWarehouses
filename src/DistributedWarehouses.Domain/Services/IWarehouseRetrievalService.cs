using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;

namespace DistributedWarehouses.Domain.Services
{
    public interface IWarehouseRetrievalService
    {
        // x Functions
        IEnumerable<WarehouseEntity> GetWarehouses();
        WarehouseEntity GetWarehouse(Guid id);
        Task<int> AddWarehouse(WarehouseEntity warehouseEntity);
        Task<int> RemoveWarehouse(Guid id);
    }
}