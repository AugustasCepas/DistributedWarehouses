using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DistributedWarehouses.Domain;
using DistributedWarehouses.Domain.Repositories;
using DistributedWarehouses.Dto;
using DistributedWarehouses.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using WarehouseEntity = DistributedWarehouses.Domain.Entities.WarehouseEntity;
using WarehouseModel = DistributedWarehouses.Infrastructure.Models.Warehouse;
using WarehouseItemEntity = DistributedWarehouses.Domain.Entities.WarehouseItemEntity;
using WarehouseItemModel = DistributedWarehouses.Infrastructure.Models.WarehouseItem;


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

        /// <summary>
        /// Return info of one warehouse
        /// How many goods are stored
        /// How many goods are reserved
        /// How much free space available (calculated in WarehouseRetrievalService)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WarehouseDto GetWarehouseInfo(Guid id)
        {
            var warehouseItems = _distributedWarehousesContext.WarehouseItems;
            var reservationItems = _distributedWarehousesContext.ReservationItems;

            var query = warehouseItems
                .GroupJoin(reservationItems, warehouseItem => new { warehouseItem.Item, warehouseItem.Warehouse },
                    reservationItem => new { reservationItem.Item, reservationItem.Warehouse },
                    (warehouseItem, reservationItemGroup) => new { warehouseItem, reservationItemGroup })
                .SelectMany(t => t.reservationItemGroup.DefaultIfEmpty(),
                    (t, reservationItem) => new { t, reservationItem })
                .Where(t => t.t.warehouseItem.Warehouse == id)
                .GroupBy(t => new { t.t.warehouseItem.Item, WarehouseQuantity = t.t.warehouseItem.Quantity },
                    t => t.reservationItem)
                .Select(g => new
                {
                    StoredQuantity = g.Key.WarehouseQuantity,
                    ReservedQuantity = g.Sum(ri => ri.Quantity) //?? 0
                })
                .GroupBy(wi => 1)
                .Select(g => new WarehouseDto
                {
                    StoredQuantity = g.Sum(wi => wi.StoredQuantity),
                    ReservedQuantity = g.Sum(wi => wi.ReservedQuantity)
                }).FirstOrDefault();

            return query;
        }

        public WarehouseEntity GetWarehouse(Guid id)
        {
            return _distributedWarehousesContext.Warehouses
                .Where(i => i.Id == id)
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

        public IEnumerable<WarehouseItemEntity> GetWarehouseItems()
        {
            return _distributedWarehousesContext.WarehouseItems.Select(i => new WarehouseItemEntity
            {
                Quantity = i.Quantity,
                Item = i.Item,
                Warehouse = i.Warehouse
            }).AsEnumerable();
        }

        public WarehouseItemEntity GetWarehouseItem(string item, Guid warehouse)
        {
            return _distributedWarehousesContext.WarehouseItems
                .Where(i => i.Item == item && i.Warehouse == warehouse)
                .Select(i => new WarehouseItemEntity
                {
                    Quantity = i.Quantity,
                    Item = i.Item,
                    Warehouse = i.Warehouse
                }).FirstOrDefault();
        }

        public async Task<int> AddWarehouseItem(WarehouseItemEntity warehouseItemEntity)
        {
            _distributedWarehousesContext.WarehouseItems.Add(new WarehouseItemModel
            {
                Quantity = warehouseItemEntity.Quantity,
                Item = warehouseItemEntity.Item,
                Warehouse = warehouseItemEntity.Warehouse
            });
            return await _distributedWarehousesContext.SaveChangesAsync();
        }

        public async Task<int> RemoveWarehouseItem(string item, Guid warehouse)
        {
            _distributedWarehousesContext.WarehouseItems.Remove(
                await _distributedWarehousesContext.FindAsync<WarehouseItemModel>(item, warehouse));
            return await _distributedWarehousesContext.SaveChangesAsync();
        }

        public async Task<int> UpdateWarehouseItemQuantity(string item, Guid warehouse, int quantity)
        {
            var warehouseItem = _distributedWarehousesContext.WarehouseItems
                .Where(i => i.Item == item && i.Warehouse == warehouse)
                .Select(i => new WarehouseItemModel
                {
                    Quantity = i.Quantity,
                    Item = i.Item,
                    Warehouse = i.Warehouse
                }).FirstOrDefault();

            if (warehouseItem == null) return 0;

            warehouseItem.Quantity -= quantity;
            _distributedWarehousesContext.WarehouseItems.Update(warehouseItem);
            return await _distributedWarehousesContext.SaveChangesAsync();

        }

        public Task<Tuple<Guid, int>> GetLargestWarehouseByFreeSpace(string sku)
        {
            var warehouseItems = _distributedWarehousesContext.WarehouseItems;
            var reservationItems = _distributedWarehousesContext.ReservationItems;

            var query = warehouseItems
                .GroupJoin(reservationItems, warehouseItem => new { warehouseItem.Item, warehouseItem.Warehouse },
                    reservationItem => new { reservationItem.Item, reservationItem.Warehouse },
                    (warehouseItem, reservationItemGroup) => new { warehouseItem, reservationItemGroup })
                .SelectMany(t => t.reservationItemGroup.DefaultIfEmpty(),
                    (t, reservationItem) => new { t, reservationItem })
                .Where(t => t.t.warehouseItem.Item == sku)
                .GroupBy(t => new { t.t.warehouseItem.Warehouse, WarehouseQuantity = t.t.warehouseItem.Quantity },
                    t => t.reservationItem)
                .Select(g =>
                    new Tuple<Guid, int>(g.Key.Warehouse, g.Key.WarehouseQuantity - g.Sum(ri => ri.Quantity)));
                // .OrderByDescending(w => w.Item2);
            return query.FirstOrDefaultAsync();
        }

        public Task<bool> ExistsAsync<T>(T id)
        {
            return _distributedWarehousesContext.Warehouses.AnyAsync(w => w.Id.Equals(id));
        }
    }
}