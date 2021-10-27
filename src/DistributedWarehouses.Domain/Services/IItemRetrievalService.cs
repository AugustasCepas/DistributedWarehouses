using System.Collections.Generic;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Dto;

namespace DistributedWarehouses.Domain.Services
{
    public interface IItemRetrievalService
    {
        // Task Functions
        IEnumerable<ItemEntity> GetItems();
        ItemDto GetItemInWarehousesInfo(string SKU);

        // Other Functions
        Task<int> AddItem(ItemEntity item);
        Task<int> RemoveItem(string SKU);
    }
}