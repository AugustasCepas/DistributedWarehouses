using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;
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

        // Task Functions
        public IEnumerable<ItemEntity> GetItems()
        {
            var result =  _itemRetrievalService.GetItems();
            // _validator.ValidateOperation(result, operationId);
            return result;
        }

        public ItemDto GetItemInWarehousesInfo(string SKU)
        {
            var result = _itemRetrievalService.GetItemInWarehousesInfo(SKU);

            return result;
        }

        // Other Functions
        public Task<int> AddItem(ItemEntity item)
        {
            var result = _itemRetrievalService.AddItem(item);

            return result;
        }

        public Task<int> RemoveItem(string SKU)
        {
            var result = _itemRetrievalService.RemoveItem(SKU);

            return result;
        }
    }
}