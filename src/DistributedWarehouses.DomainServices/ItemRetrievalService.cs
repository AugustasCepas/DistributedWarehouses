using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DistributedWarehouses.Domain;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Domain.Repositories;
using DistributedWarehouses.Domain.RetrievalServices;
using DistributedWarehouses.Dto;

namespace DistributedWarehouses.DomainServices
{
    public class ItemRetrievalService : IItemRetrievalService
    {
        private readonly IItemRepository _itemRepository;


        public ItemRetrievalService(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public IEnumerable<ItemEntity> GetItems()
        {
            return _itemRepository.GetItems();
        }

        /// <summary>
        /// Return info about one SKU
        /// How many items left in each warehouse
        /// How many items are reserved
        /// TODO: How many items are planned to be delivered soon
        /// </summary>
        /// <param name="sku"></param>
        /// <returns></returns>
        public ItemDto GetItemInWarehousesInfo(string sku)
        {
            var item = _itemRepository.GetItem(sku);
            var result = new ItemDto
            {
                SKU = item.SKU,
                Title = item.Title,
                InWarehouses = _itemRepository.GetItemInWarehousesInfo(sku)
            };

            return result;
        }

        public Task<int> AddItem(ItemEntity item)
        {
            return _itemRepository.AddItem(item);
        }

        public Task<int> RemoveItem(string sku)
        {
            return _itemRepository.RemoveItem(sku);
        }
    }
}