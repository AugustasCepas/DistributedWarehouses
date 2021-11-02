using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Dto;

namespace DistributedWarehouses.Domain.Services
{
    public interface IItemService
    {
        // Task Functions
        IEnumerable<ItemEntity> GetItems();
        Task<ItemDto> GetItemInWarehousesInfo(string sku);

        // Other Functions
        Task<int> AddItem(ItemEntity item);
        Task<int> RemoveItem(string sku);
    }
}