using System.Collections.Generic;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Dto;

namespace DistributedWarehouses.Domain.Repositories
{
    public interface IItemRepository
    {
        // Task Functions
        IEnumerable<ItemEntity> GetItems();
        IEnumerable<ItemInWarehousesInfoDto> GetItemsInWarehouses(string SKU);
        ItemEntity GetItem(string SKU);

        // Other Functions
        Task<int> AddItem(ItemEntity item);
        Task<int> RemoveItem(string SKU);
    }
}