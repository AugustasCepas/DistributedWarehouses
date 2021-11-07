using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Domain.Repositories;
using DistributedWarehouses.Dto;
using DistributedWarehouses.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using WarehouseModel = DistributedWarehouses.Infrastructure.Models.Warehouse;
using WarehouseItemModel = DistributedWarehouses.Infrastructure.Models.WarehouseItem;


namespace DistributedWarehouses.Infrastructure.Repositories
{
    public class WarehouseRepository : IWarehouseRepository
    {
        private readonly DistributedWarehousesContext _distributedWarehousesContext;
        private readonly IMapper _mapper;

        public WarehouseRepository(DistributedWarehousesContext distributedWarehousesContext, IMapper mapper)
        {
            _distributedWarehousesContext = distributedWarehousesContext;
            _mapper = mapper;
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

        public IEnumerable<WarehouseItemEntity> GetWarehouseItems(Guid id)
        {
            return _distributedWarehousesContext.WarehouseItems
                .Where(w => w.Warehouse == id)
                .Select(i => new WarehouseItemEntity
                {
                    Quantity = i.Quantity,
                    Item = i.Item,
                    Warehouse = i.Warehouse
                }).ToList();
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

        public async Task<WarehouseItemEntity> AddWarehouseItem(WarehouseItemEntity warehouseItemEntity)
        {
            await _distributedWarehousesContext.WarehouseItems
                .Upsert(_mapper.Map<WarehouseItem>(warehouseItemEntity))
                .On(wi => new { wi.Item, wi.Warehouse }).WhenMatched(wi => new WarehouseItem
                {
                    Quantity = wi.Quantity + warehouseItemEntity.Quantity
                }).RunAsync(CancellationToken.None);
            return warehouseItemEntity;
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

        public Task<WarehouseInformation> GetWarehouseByItem(string sku, string property)
        {
            var warehouses = _distributedWarehousesContext.Warehouses;
            var warehouseItems = _distributedWarehousesContext.WarehouseItems;
            var specificWarehouseItems = _distributedWarehousesContext.WarehouseItems.GroupBy(wi => new { wi.Warehouse, wi.Item }).Select(g =>
                    new WarehouseItem { Warehouse = g.Key.Warehouse, Item = g.Key.Item, Quantity = g.Sum(g => g.Quantity) });
            var specificReservationItems = _distributedWarehousesContext.ReservationItems.GroupBy(ri => new { ri.Warehouse, ri.Item }).Select(g =>
                    new ReservationItem { Warehouse = g.Key.Warehouse, Item = g.Key.Item, Quantity = g.Sum(g => g.Quantity) });


            var query = (warehouses
                .GroupJoin(warehouseItems, warehouse => new { warehouse.Id },
                    allWarehouseItem => new { Id = allWarehouseItem.Warehouse },
                    (warehouse, awis) => new { warehouse, awis })
                .SelectMany(t => t.awis.DefaultIfEmpty(), (t, awi) => new { t, awi })

                .GroupJoin(specificWarehouseItems, t => new { t.t.warehouse.Id, t.awi.Item, SKU = sku },
                    allWarehouseItem => new
                        { Id = allWarehouseItem.Warehouse, Item = allWarehouseItem.Item, SKU = allWarehouseItem.Item },
                    (t, wis) => new { t, wis })
                .SelectMany(t => t.wis.DefaultIfEmpty(), (t, wi) => new { t, wi })

                .GroupJoin(specificReservationItems,
                    t => new { t.t.t.t.warehouse.Id, t.wi.Item },
                    reservationItem => new { Id = reservationItem.Warehouse, reservationItem.Item },
                    (t, rwis) => new { t, rwis })
                .SelectMany(t => t.rwis.DefaultIfEmpty(), (t, rwi) => new { t, rwi })

                .GroupBy(t => new { t.t.t.t.t.t.warehouse.Id, t.t.t.t.t.t.warehouse.Capacity })

                .Select(g =>
                    new WarehouseInformation
                    {
                        Id = g.Key.Id,
                        Capacity = g.Key.Capacity,
                        FreeQuantity = g.Key.Capacity - g.Sum(w => w.t.t.t.t.awi.Quantity),
                        StoredItemQuantity = g.Sum(g => g.t.t.wi.Quantity),
                        AvailableItemQuantity =
                            g.Sum(g => g.t.t.wi.Quantity) - g.Sum(g => g.rwi.Quantity)
                    }));

            var result = query.ToList()
                .OrderByDescending(w => typeof(WarehouseInformation).GetProperty(property).GetValue(w))
                .FirstOrDefault();
            return Task.FromResult(result);
        }

        public Task<WarehouseItemEntity> GetLargestWarehouseByFreeItemsQuantityAsync(string sku)
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
                    new WarehouseItem
                    {
                        Warehouse = g.Key.Warehouse,
                        Quantity = g.Key.WarehouseQuantity - g.Sum(ri => ri.Quantity)
                    });
            return _mapper
                .ProjectTo<WarehouseItemEntity>(query).OrderByDescending(wi => wi.Quantity).FirstOrDefaultAsync();
        }

        public Task<bool> ExistsAsync<T>(T id)
        {
            return _distributedWarehousesContext.Warehouses.AnyAsync(w => w.Id.Equals(id));
        }

        public Task<WarehouseItemEntity> GetWarehouseByFreeSpace()
        {
            var warehouseItems = _distributedWarehousesContext.WarehouseItems;
            var warehouses = _distributedWarehousesContext.Warehouses;

            var query = warehouseItems
                .GroupJoin(warehouses, warehouseItem => warehouseItem.Warehouse, warehouse => warehouse.Id,
                    (warehouseItem, warehouseGroup) => new { warehouseItem, warehouseGroup })
                .SelectMany(t => t.warehouseGroup.DefaultIfEmpty(), (t, warehouse) => new { t, warehouse })
                .GroupBy(
                    t => new
                    {
                        WarehouseId = t.t.warehouseItem.Warehouse,
                        WarehouseCapacity = t.warehouse.Capacity,
                        WarehouseQuantity = t.t.warehouseItem.Quantity
                    }, t => t.warehouse)
                .Select(g =>
                    new WarehouseItem
                    {
                        Warehouse = g.Key.WarehouseId,
                        Quantity = g.Key.WarehouseCapacity - g.Key.WarehouseQuantity
                    }
                );


            return _mapper.ProjectTo<WarehouseItemEntity>(query).OrderByDescending(wi => wi.Quantity)
                .FirstOrDefaultAsync();
        }

        public Task<int> AddInvoiceItemsToWarehouseAsync(InvoiceItemEntity invoiceItem)
        {
            return _distributedWarehousesContext.WarehouseItems
                .Upsert(_mapper.Map<WarehouseItemModel>(invoiceItem))
                .On(wi => new { wi.Item, wi.Warehouse }).WhenMatched((wiInDb, wiNew) => new WarehouseItem
                {
                    Quantity = wiInDb.Quantity + wiNew.Quantity
                }).RunAsync(CancellationToken.None);
        }

        public async Task Add<T>(T entity) where T : DistributableItemEntity
        {
            await AddWarehouseItem(entity as WarehouseItemEntity);
        }
    }
}