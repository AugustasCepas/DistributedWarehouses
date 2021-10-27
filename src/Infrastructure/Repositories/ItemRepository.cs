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

        // Task Functions
        public IEnumerable<ItemEntity> GetItems()
        {
            return _distributedWarehousesContext.Items.Select(i => new ItemEntity
            {
                SKU = i.Sku,
                Title = i.Title
            }).AsEnumerable();
        }

        public IEnumerable<ItemInWarehousesInfoDto> GetItemsInWarehouses(string sku)
        {
            var items = _distributedWarehousesContext.Items;
            var warehouseItems = _distributedWarehousesContext.WarehouseItems;
            var reservationItems = _distributedWarehousesContext.ReservationItems;

            var query =
                from item in items
                join warehouseItem in warehouseItems on item.Sku equals warehouseItem.Item into warehouseItemGroup
                from warehouseItem in warehouseItemGroup.DefaultIfEmpty()
                join reservationItem in reservationItems on new { sku = item.Sku, warehouseItem.Warehouse } equals new
                    { sku = reservationItem.Item, reservationItem.Warehouse } into reservationItemGroup
                from reservationItem in reservationItemGroup.DefaultIfEmpty()
                where item.Sku == sku
                group reservationItem by new
                {
                    item.Sku,
                    warehouseItem.Warehouse,
                    WarehouseQuantity = warehouseItem.Quantity,
                    reservationItem.Item
                }
                into g
                select new ItemInWarehousesInfoDto
                {
                    WarehouseId = g.Key.Warehouse,
                    StoredQuantity = g.Key.WarehouseQuantity,
                    ReservedQuantity = g.Sum(ri => ri.Quantity) ?? 0
                };

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

        // Other Functions
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