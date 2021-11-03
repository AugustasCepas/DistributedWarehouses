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
        Task<ItemDto> GetItemInWarehousesInfoAsync(string sku);

        // Other Functions
        Task<ItemEntity> AddItemAsync(ItemEntity item);
        Task RemoveItemAsync(string sku);
    }
}