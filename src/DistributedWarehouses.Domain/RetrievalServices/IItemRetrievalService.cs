using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Dto;

namespace DistributedWarehouses.Domain.RetrievalServices
{
    public interface IItemRetrievalService
    {
        IEnumerable<ItemEntity> GetItems();
        ItemDto GetItemInWarehousesInfo(string sku);
        Task<int> AddItem(ItemEntity item);
        Task<int> RemoveItem(string sku);
    }
}