using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DistributedWarehouses.Domain;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Infrastructure.Models;
using DistributedWarehouses.Infrastructure.Models.Extensions;
using Item = DistributedWarehouses.Domain.Entities.Item;
using WarehouseItemEntity = DistributedWarehouses.Domain.Entities.WarehouseItem;
using WarehouseItemModel = DistributedWarehouses.Infrastructure.Models.WarehouseItem;

namespace DistributedWarehouses.Infrastructure.Repositories
{
    public class WarehouseItemRepository : IWarehouseItemRepository
    {
        private readonly DistributedWarehousesContext _distributedWarehouseContext;

        public WarehouseItemRepository(DistributedWarehousesContext distributedWarehouseContext)
        {
            _distributedWarehouseContext = distributedWarehouseContext;
        }
        
        public IEnumerable<WarehouseItemEntity> GetWarehouseItem(string sku)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> AddWarehouseItem(Item item)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> RemoveWarehouseItem(string sku)
        {
            throw new System.NotImplementedException();
        }

        // public IEnumerable<WarehouseItemEntity> GetWarehouseItem(string sku)
        // {
        //     return _distributedWarehouseItemsContext.WarehouseItems
        //         .Where(i => i.Sku == sku)
        //         .Select(i => new WarehouseItemEntity
        //         {
        //             SKU = i.Sku,
        //             Title = i.Title
        //         }).AsEnumerable();
        // }

        public Task<int> AddItem(WarehouseItemEntity item)
        {
            _distributedWarehouseContext.WarehouseItems.Add(new WarehouseItemModel
            {
                Quantity = item.Quantity,
                Item = item.Item.SKU
            });
            return _distributedWarehouseContext.SaveChangesAsync();
        }

        public async Task<int> RemoveItem(string sku)
        {
            _distributedWarehouseContext.WarehouseItems.Remove(await _distributedWarehouseContext.FindAsync<WarehouseItemModel>(sku));
            return await _distributedWarehouseContext.SaveChangesAsync();
        }
    
    }
}
