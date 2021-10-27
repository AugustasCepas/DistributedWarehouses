using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Domain.Services;

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

        public WarehouseEntity GetWarehouse(Guid id)
        {
            var result = _warehouseRetrievalService.GetWarehouse(id);

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
    }
}