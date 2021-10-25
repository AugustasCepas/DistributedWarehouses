using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DistributedWarehouses.Domain;
using DistributedWarehouses.Dto;
using DistributedWarehouses.Infrastructure.Models;
using ItemEntity = DistributedWarehouses.Domain.Entities.Item;
using ItemModel = DistributedWarehouses.Infrastructure.Models.Item;

namespace DistributedWarehouses.Infrastructure.Repositories
{
    public class ItemsRepository : IItemsRepository
    {
        private readonly DistributedWarehousesContext _distributedWarehousesContext;

        public ItemsRepository(DistributedWarehousesContext distributedWarehousesContext)
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

        public ItemEntity GetItem(string sku)
        {
            return _distributedWarehousesContext.Items
                .Where(i => i.Sku == sku)
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

        public IEnumerable<ItemInWarehousesInfoDto> GetItemsInWarehouses(string sku)
        {
            using (_distributedWarehousesContext)
            {
                var items = _distributedWarehousesContext.Set<Item>();
                var warehouseItems = _distributedWarehousesContext.Set<WarehouseItem>();
                var reservationItems = _distributedWarehousesContext.Set<ReservationItem>();
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
        }
    }
}