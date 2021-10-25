using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DistributedWarehouses.Domain;
using DistributedWarehouses.Infrastructure.Models;
using Warehouse = DistributedWarehouses.Domain.Entities.Warehouse;
using WarehouseModel = DistributedWarehouses.Infrastructure.Models.Warehouse;


namespace DistributedWarehouses.Infrastructure.Repositories
{
    public class WarehouseRepository : IWarehouseRepository
    {
        private readonly DistributedWarehousesContext _distributedWarehousesContext;

        public WarehouseRepository(DistributedWarehousesContext distributedWarehousesContext)
        {
            _distributedWarehousesContext = distributedWarehousesContext;
        }

        public IEnumerable<Warehouse> GetWarehouses()
        {
            return _distributedWarehousesContext.Warehouses.Select(i => new Warehouse
            {
                Id = i.Id,
                Address = i.Address
            }).AsEnumerable();
        }

        public IEnumerable<Warehouse> GetWarehouse(Guid id)
        {
            return _distributedWarehousesContext.Warehouses
                .Where(w => w.Id == id)
                .Select(i => new Warehouse
                {
                    Id = i.Id,
                    Address = i.Address
                }).AsEnumerable();
        }

        public Task<int> AddWarehouse(Warehouse warehouse)
        {
            _distributedWarehousesContext.Warehouses.Add(new WarehouseModel
            {
                Id = warehouse.Id,
                Address = warehouse.Address
            });
            return _distributedWarehousesContext.SaveChangesAsync();
        }

        public async Task<int> RemoveWarehouse(Guid id)
        {
            _distributedWarehousesContext.Warehouses.Remove(
                await _distributedWarehousesContext.FindAsync<WarehouseModel>(id));
            return await _distributedWarehousesContext.SaveChangesAsync();
        }
    }
}