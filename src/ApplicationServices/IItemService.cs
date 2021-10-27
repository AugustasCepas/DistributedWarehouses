using System.Collections.Generic;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Dto;

namespace DistributedWarehouses.ApplicationServices
{
    public interface IItemService
    {
        // Task Functions
        IEnumerable<ItemEntity> GetItems();
        ItemDto GetItem(string SKU);

        // Other Functions
        Task<int> AddItem(ItemEntity item);
        Task<int> RemoveItem(string SKU);
    }
}
