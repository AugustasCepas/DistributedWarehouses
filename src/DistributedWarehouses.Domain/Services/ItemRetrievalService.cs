using System.Collections.Generic;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Domain.Repositories;
using DistributedWarehouses.Dto;

namespace DistributedWarehouses.Domain.Services
{
    public class ItemRetrievalService : IItemRetrievalService
    {
        private readonly IItemRepository _itemRepository;

        public ItemRetrievalService(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        // Task Functions
        public IEnumerable<ItemEntity> GetItems()
        {
            return _itemRepository.GetItems();
        }

        public ItemDto GetItemInWarehousesInfo(string SKU)
        {
            var item = _itemRepository.GetItem(SKU);
            var result = new ItemDto
            {
                SKU = item.SKU,
                Title = item.Title,
                InWarehouses = _itemRepository.GetItemInWarehousesInfo(SKU)
            };

            return result;

        }

        // Other Functions
        public Task<int> AddItem(ItemEntity item)
        {
            return _itemRepository.AddItem(item);
        }

        public Task<int> RemoveItem(string SKU)
        {
            return _itemRepository.RemoveItem(SKU);
        }
    }
}