using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Domain.Exceptions;
using DistributedWarehouses.Domain.Repositories;
using DistributedWarehouses.Domain.RetrievalServices;
using DistributedWarehouses.Domain.Services;
using DistributedWarehouses.Domain.Validators;
using DistributedWarehouses.Dto;

namespace DistributedWarehouses.ApplicationServices
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;
        private readonly IValidator<(bool, string), IItemRepository> _skuValidator;
        private readonly IValidator<ItemEntity, IItemRepository> _itemValidator;
        private readonly IMappingService _mappingService;

        public ItemService(IItemRepository itemRepository, IValidator<(bool, string), IItemRepository> skuValidator,
            IValidator<ItemEntity, IItemRepository> itemValidator, IMappingService mappingService)
        {
            _itemRepository = itemRepository;
            _skuValidator = skuValidator;
            _itemValidator = itemValidator;
            _mappingService = mappingService;
        }

        public IEnumerable<ItemEntity> GetItems()
        {
            var result = _itemRepository.GetItems();
            return result;
        }

        public async Task<ItemDto> GetItemInWarehousesInfoAsync(string sku)
        {
            await _skuValidator.ValidateAsync((false, sku));
            var item = _mappingService.Map<ItemDto>(await _itemRepository.GetItemAsync(sku));
            item.InWarehouses =
                _mappingService.Map<IEnumerable<ItemInWarehousesInfoDto>>(_itemRepository.GetItemInWarehousesInfo(sku));
            return item;
        }

        public async Task<ItemEntity> AddItemAsync(ItemEntity item)
        {
            await _skuValidator.ValidateAsync((true, item.SKU));
            await _itemValidator.ValidateAsync(item);
            var result = await _itemRepository.AddItemAsync(item);
            return result == 1 ? item : null;
        }

        public async Task RemoveItemAsync(string sku)
        {
            await _skuValidator.ValidateAsync((false, sku));
            await _itemRepository.RemoveItemAsync(sku);
        }
    }
}