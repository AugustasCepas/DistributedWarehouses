using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Domain.RetrievalServices;
using DistributedWarehouses.Domain.Services;
using DistributedWarehouses.Dto;

namespace DistributedWarehouses.ApplicationServices
{
    public class WarehouseService : IWarehouseService
    {
        private readonly IWarehouseRetrievalService _warehouseRetrievalService;

        public WarehouseService(IWarehouseRetrievalService warehouseRetrievalService)
        {
            _warehouseRetrievalService = warehouseRetrievalService;
        }

        public IEnumerable<WarehouseEntity> GetWarehouses()
        {
            var result = _warehouseRetrievalService.GetWarehouses();
            // _validator.ValidateOperation(result, operationId);

            return result;
        }

        public WarehouseDto GetWarehouseInfo(Guid id)
        {
            var result = _warehouseRetrievalService.GetWarehouseInfo(id);

            return result;
        }

        public Task<int> AddWarehouse(WarehouseEntity warehouseEntity)
        {
            var result = _warehouseRetrievalService.AddWarehouse(warehouseEntity);

            return result;
        }

        public Task<int> RemoveWarehouse(Guid id)
        {
            var result = _warehouseRetrievalService.RemoveWarehouse(id);

            return result;
        }

        public IEnumerable<WarehouseItemEntity> GetWarehouseItems()
        {
            var result = _warehouseRetrievalService.GetWarehouseItems();

            return result;
        }

        public WarehouseItemEntity GetWarehouseItem(string item, Guid warehouse)
        {
            return _warehouseRetrievalService.GetWarehouseItem(item, warehouse);
        }


        public Task<int> AddWarehouseItem(WarehouseItemEntity warehouseItemEntity)
        {
            var result = _warehouseRetrievalService.AddWarehouseItem(warehouseItemEntity);

            return result;
        }

        public Task<int> RemoveWarehouseItem(string item, Guid warehouse)
        {
            return _warehouseRetrievalService.RemoveWarehouseItem(item, warehouse);
        }

        public Task<int> ItemSold(ItemSellDto dto)
        {
            var result = _warehouseRetrievalService.ItemSold(dto);
            return result;
        }
    }
}