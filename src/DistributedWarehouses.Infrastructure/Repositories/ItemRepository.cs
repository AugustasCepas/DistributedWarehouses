using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Repositories;
using DistributedWarehouses.Dto;
using DistributedWarehouses.Infrastructure.Models;
using ItemEntity = DistributedWarehouses.Domain.Entities.ItemEntity;
using ItemModel = DistributedWarehouses.Infrastructure.Models.Item;

namespace DistributedWarehouses.Infrastructure.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly DistributedWarehousesContext _distributedWarehousesContext;

        public ItemRepository(DistributedWarehousesContext distributedWarehousesContext)
        {
            _distributedWarehousesContext = distributedWarehousesContext;
        }

        public IEnumerable<ItemEntity> GetItems()
        {
            return _distributedWarehousesContext.Items.Select(i => new ItemEntity
            {
                SKU = i.Sku,
                Title = i.Title
            }).AsEnumerable();
        }

        /// <summary>
        /// Return info about one SKU
        /// How many items left in each warehouse
        /// How many items are reserved
        /// TODO: How many items are planned to be delivered soon
        /// </summary>
        /// <param name="sku"></param>
        /// <returns></returns>
        public IEnumerable<ItemInWarehousesInfoDto> GetItemInWarehousesInfo(string sku)
        {
            var warehouseItems = _distributedWarehousesContext.WarehouseItems;
            var reservationItems = _distributedWarehousesContext.ReservationItems;

            var query = warehouseItems
                .GroupJoin(reservationItems, warehouseItem => new {warehouseItem.Item, warehouseItem.Warehouse},
                    reservationItem => new {reservationItem.Item, reservationItem.Warehouse},
                    (warehouseItem, reservationItemGroup) => new {warehouseItem, reservationItemGroup})
                .SelectMany(@t => @t.reservationItemGroup.DefaultIfEmpty(),
                    (@t, reservationItem) => new {@t, reservationItem})
                .Where(@t => @t.@t.warehouseItem.Item == sku)
                .GroupBy(@t => new {@t.@t.warehouseItem.Warehouse, WarehouseQuantity = @t.@t.warehouseItem.Quantity},
                    @t => @t.reservationItem)
                .Select(g => new ItemInWarehousesInfoDto
                {
                    WarehouseId = g.Key.Warehouse,
                    StoredQuantity = g.Key.WarehouseQuantity,
                    ReservedQuantity = g.Sum(ri => ri.Quantity)
                });

            return query.AsEnumerable();
        }

        public ItemEntity GetItem(string SKU)
        {
            return _distributedWarehousesContext.Items
                .Where(i => i.Sku == SKU)
                .Select(i => new ItemEntity
                {
                    SKU = i.Sku,
                    Title = i.Title
                }).FirstOrDefault();
        }

        public Task<int> AddItem(ItemEntity item)
        {
            _distributedWarehousesContext.Items.Add(new ItemModel
            {
                Sku = item.SKU,
                Title = item.Title
            });
            return _distributedWarehousesContext.SaveChangesAsync();
        }

        public async Task<int> RemoveItem(string sku)
        {
            _distributedWarehousesContext.Items.Remove(await _distributedWarehousesContext.FindAsync<ItemModel>(sku));
            return await _distributedWarehousesContext.SaveChangesAsync();
        }
    }
}