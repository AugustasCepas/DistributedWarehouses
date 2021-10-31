using System.Collections.Generic;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Dto;

namespace DistributedWarehouses.Domain.Repositories
{
    public interface IItemRepository
    {
        IEnumerable<ItemEntity> GetItems();
        IEnumerable<ItemInWarehousesInfoDto> GetItemInWarehousesInfo(string SKU);
        ItemEntity GetItem(string SKU);
        Task<int> AddItem(ItemEntity item);
        Task<int> RemoveItem(string SKU);
        public Task<bool> ExistsAsync(string sku);
    }
}