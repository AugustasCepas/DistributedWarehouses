using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Domain.Repositories;
using DistributedWarehouses.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using ItemEntity = DistributedWarehouses.Domain.Entities.ItemEntity;
using ItemModel = DistributedWarehouses.Infrastructure.Models.Item;

namespace DistributedWarehouses.Infrastructure.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly DistributedWarehousesContext _distributedWarehousesContext;
        private readonly IMapper _mapper;

        public ItemRepository(DistributedWarehousesContext distributedWarehousesContext, IMapper mappingService)
        {
            _distributedWarehousesContext = distributedWarehousesContext;
            _mapper = mappingService;
        }

        public IEnumerable<ItemEntity> GetItems()
        {
            return _mapper.ProjectTo<ItemEntity>(_distributedWarehousesContext.Items).AsEnumerable();
        }

        /// <summary>
        /// Return info about one SKU
        /// How many items left in each warehouse
        /// How many items are reserved
        /// TODO: How many items are planned to be delivered soon
        /// </summary>
        /// <param name="sku"></param>
        /// <returns></returns>
        public IEnumerable<Tuple<Guid, int, int>> GetItemInWarehousesInfo(string sku)
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
                    new Tuple<Guid, int, int>(g.Key.Warehouse, g.Key.WarehouseQuantity, g.Sum(ri => ri.Quantity)));
            return query.AsEnumerable();
        }

        public Task<ItemEntity> GetItemAsync(string sku)
        {
            return _mapper.ProjectTo<ItemEntity>(_distributedWarehousesContext.Items)
                .FirstOrDefaultAsync(i => i.SKU == sku);
        }

        public Task<bool> ExistsAsync<T>(T sku)
        {
            return _distributedWarehousesContext.Items.AnyAsync(i => i.Sku.Equals(sku));
        }

        public Task Add<T>(T entity) where T : DistributableItemEntity
        {
            throw new NotImplementedException();
        }
    }
}