using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Dto;

namespace DistributedWarehouses.Domain.Repositories
{
    public interface IItemRepository : IRepository
    {
        IEnumerable<ItemEntity> GetItems();
        IEnumerable<Tuple<Guid, int, int>> GetItemInWarehousesInfo(string SKU);
        Task<ItemEntity> GetItemAsync(string sku);
        Task<int> AddItemAsync(ItemEntity item);
        Task<int> RemoveItemAsync(string SKU);
    }
}