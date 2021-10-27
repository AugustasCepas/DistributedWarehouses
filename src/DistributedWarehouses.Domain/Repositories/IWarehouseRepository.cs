using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;

namespace DistributedWarehouses.Domain
{
    public interface IWarehouseRepository
    {
        public IEnumerable<WarehouseEntity> GetWarehouses();
        public WarehouseEntity GetWarehouse(Guid Id);
        public Task<int> AddWarehouse(WarehouseEntity warehouseEntity);
        public Task<int> RemoveWarehouse(Guid Id);

    }
}