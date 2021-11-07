using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Domain.Exceptions;
using DistributedWarehouses.Domain.Repositories;
using DistributedWarehouses.Domain.Resources;
using DistributedWarehouses.Domain.Services;
using DistributedWarehouses.Domain.Validators;
using DistributedWarehouses.Dto;
using FluentValidation;

namespace DistributedWarehouses.ApplicationServices
{
    public class WarehouseService : IWarehouseService
    {
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IValidator<Guid, IWarehouseRepository> _warehouseGuidValidator;
        private readonly IValidator<string, IItemRepository> _skuValidator;
        private readonly IMappingService _mappingService;

        public WarehouseService(IWarehouseRepository warehouseRepository,
            IValidator<Guid, IWarehouseRepository> warehouseGuidValidator,
            IValidator<string, IItemRepository> skuValidator, IMappingService mappingService)
        {
            _warehouseRepository = warehouseRepository;
            _warehouseRepository = warehouseRepository;
            _warehouseGuidValidator = warehouseGuidValidator;
            _skuValidator = skuValidator;
            _mappingService = mappingService;
        }

        public IEnumerable<WarehouseEntity> GetWarehouses()
        {
            var result = _warehouseRepository.GetWarehouses();
            return result;
        }

        public async Task<WarehouseDto> GetWarehouseInfo(Guid id)
        {
            await _warehouseGuidValidator.ValidateAsync(id, false);
            var result = _mappingService.Map<WarehouseDto>(_warehouseRepository.GetWarehouse(id));
            var warehouseInfo = _warehouseRepository.GetWarehouseInfo(id);
            result.FreeQuantity -= warehouseInfo.StoredQuantity;
            result.StoredQuantity = warehouseInfo.StoredQuantity;
            result.ReservedQuantity = warehouseInfo.ReservedQuantity;

            return result;
        }

        public async Task<WarehouseItemEntity> AddWarehouseItem(Guid warehouse, string sku, int quantity)
        {
            await _skuValidator.ValidateAsync(sku, false);
            await _warehouseGuidValidator.ValidateAsync(warehouse, false);

            if (quantity <= 0)
            {
                throw new BadRequestException(string.Format(ErrorMessageResource.NotSupported, "quantity",
                    "Larger than 0"));
            }

            WarehouseItemEntity warehouseItemEntity = new WarehouseItemEntity
            {
                Warehouse = warehouse,
                Item = sku,
                Quantity = quantity
            };

            if (_warehouseRepository.GetWarehouseItem(sku, warehouse) == null)
            {
                return await Task.FromResult<WarehouseItemEntity>(null);
            }

            await _warehouseRepository.AddWarehouseItem(warehouseItemEntity);

             return _warehouseRepository.GetWarehouseItem(sku, warehouse);
        }
    }
}