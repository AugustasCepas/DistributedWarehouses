using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Domain.Exceptions;
using DistributedWarehouses.Domain.RetrievalServices;
using DistributedWarehouses.Domain.Services;
using DistributedWarehouses.Dto;
using FluentValidation;

namespace DistributedWarehouses.ApplicationServices
{
    public class ItemService : IItemService
    {
        private readonly IItemRetrievalService _itemRetrievalService;
        private readonly IValidator<string> _validator;

        public ItemService(IItemRetrievalService itemRetrievalService, IValidator<string> validator)
        {
            _itemRetrievalService = itemRetrievalService;
            _validator = validator;
        }

        public IEnumerable<ItemEntity> GetItems()
        {
            var result = _itemRetrievalService.GetItems();
            // _validator.ValidateOperation(result, operationId);
            return result;
        }

        public async Task<ItemDto> GetItemInWarehousesInfo(string sku)
        {
            var validationResult = await _validator.ValidateAsync(sku);
            if (!validationResult.IsValid)
            {
                throw new BaseException(validationResult.Errors.First().ErrorMessage, int.Parse(validationResult.Errors.First().ErrorCode));
            }
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