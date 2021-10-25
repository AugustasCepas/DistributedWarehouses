using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;

namespace DistributedWarehouses.Domain
{
    public interface IWarehouseRepository
    {
        public IEnumerable<Warehouse> GetWarehouses();
        public IEnumerable<Warehouse> GetWarehouse(Guid Id);
        public Task<int> AddWarehouse(Warehouse warehouse);
        public Task<int> RemoveWarehouse(Guid Id);

    }
}