using System.Collections.Generic;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Dto;

namespace DistributedWarehouses.Domain.RetrievalServices
{
    public interface IItemRetrievalService
    {
        IEnumerable<ItemEntity> GetItems();
        ItemDto GetItemInWarehousesInfo(string SKU);
        Task<int> AddItem(ItemEntity item);
        Task<int> RemoveItem(string SKU);
    }
}