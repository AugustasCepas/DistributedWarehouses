using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Domain.RetrievalServices;
using DistributedWarehouses.Domain.Services;
using DistributedWarehouses.Dto;

namespace DistributedWarehouses.ApplicationServices
{
    public class ItemService : IItemService
    {
        private readonly IItemRetrievalService _itemRetrievalService;

        public ItemService(IItemRetrievalService itemRetrievalService)
        {
            _itemRetrievalService = itemRetrievalService;
        }

        public IEnumerable<ItemEntity> GetItems()
        {
            var result = _itemRetrievalService.GetItems();

            return result;
        }

        public ItemDto GetItemInWarehousesInfo(string sku)
        {
            var result = _itemRetrievalService.GetItemInWarehousesInfo(sku);

            return result;
        }

        public Task<int> AddItem(ItemEntity item)
        {
            var result = _itemRetrievalService.AddItem(item);

            return result;
        }

        public Task<int> RemoveItem(string sku)
        {
            var result = _itemRetrievalService.RemoveItem(sku);

            return result;
        }



    }
}