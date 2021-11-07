using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;

namespace DistributedWarehouses.Domain.Repositories
{
    public interface IItemRepository : IRepository
    {
        IEnumerable<ItemEntity> GetItems();
        IEnumerable<Tuple<Guid, int, int>> GetItemInWarehousesInfo(string sku);
        Task<ItemEntity> GetItemAsync(string sku);
    }
}