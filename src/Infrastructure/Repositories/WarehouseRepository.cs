using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DistributedWarehouses.Domain;
using DistributedWarehouses.Infrastructure.Models;
using WarehouseEntity = DistributedWarehouses.Domain.Entities.WarehouseEntity;
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

        public IEnumerable<WarehouseEntity> GetWarehouses()
        {
            return _distributedWarehousesContext.Warehouses.Select(i => new WarehouseEntity
            {
                Id = i.Id,
                Address = i.Address,
                Capacity = i.Capacity
            }).AsEnumerable();
        }

        public WarehouseEntity GetWarehouse(Guid id)
        {
            // SELECT SUM(Quantity), SUM(ReservedQuantity) FROM(
            //     SELECT WI.Quantity, SUM(RI.Quantity) as ReservedQuantity FROM WarehouseItem WI
            // LEFT JOIN ReservationItem RI ON RI.Item = WI.Item AND RI.Warehouse = WI.Warehouse
            // WHERE WI.Warehouse = '3FA85F64-5717-4562-B3FC-2C963F66AFA6'
            // GROUP BY WI.Item, WI.Quantity
            //     ) sub
            return _distributedWarehousesContext.Warehouses
                .Where(w => w.Id == id)
                .Select(i => new WarehouseEntity
                {
                    Id = i.Id,
                    Address = i.Address,
                    Capacity = i.Capacity
                }).FirstOrDefault();
        }

        public async Task<int> AddWarehouse(WarehouseEntity warehouseEntity)
        {
            _distributedWarehousesContext.Warehouses.Add(new WarehouseModel
            {
                Id = warehouseEntity.Id,
                Address = warehouseEntity.Address,
                Capacity = warehouseEntity.Capacity
            });
            return await _distributedWarehousesContext.SaveChangesAsync();
        }

        public async Task<int> RemoveWarehouse(Guid id)
        {
            _distributedWarehousesContext.Warehouses.Remove(
                await _distributedWarehousesContext.FindAsync<WarehouseModel>(id));
            return await _distributedWarehousesContext.SaveChangesAsync();
        }
    }
}