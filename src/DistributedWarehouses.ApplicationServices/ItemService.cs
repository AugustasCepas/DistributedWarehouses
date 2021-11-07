using System.Collections.Generic;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Domain.Repositories;
using DistributedWarehouses.Domain.Services;
using DistributedWarehouses.Domain.Validators;
using DistributedWarehouses.Dto;

namespace DistributedWarehouses.ApplicationServices
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;
        private readonly IValidator<string, IItemRepository> _skuValidator;
        private readonly IMappingService _mappingService;

        public ItemService(IItemRepository itemRepository, IValidator<string, IItemRepository> skuValidator, IMappingService mappingService)
        {
            _itemRepository = itemRepository;
            _skuValidator = skuValidator;
            _mappingService = mappingService;
        }

        public IEnumerable<ItemEntity> GetItems()
        {
            var result = _itemRepository.GetItems();
            return result;
        }

        public async Task<ItemDto> GetItemInWarehousesInfoAsync(string sku)
        {
            await _skuValidator.ValidateAsync(sku, false);
            var item = _mappingService.Map<ItemDto>(await _itemRepository.GetItemAsync(sku));
            item.InWarehouses =
                _mappingService.Map<IEnumerable<ItemInWarehousesInfoDto>>
                    (_itemRepository.GetItemInWarehousesInfo(sku));
            return item;
        }
    }
}