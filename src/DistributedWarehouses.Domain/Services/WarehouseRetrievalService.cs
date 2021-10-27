using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Domain.Repositories;

namespace DistributedWarehouses.Domain.Services
{
    public class WarehouseRetrievalService : IWarehouseRetrievalService
    {
        private readonly IWarehouseRepository _warehouseRepository;

        public WarehouseRetrievalService(IWarehouseRepository warehouseRepository)
        {
            _warehouseRepository = warehouseRepository;
        }

        public IEnumerable<WarehouseEntity> GetWarehouses()
        {
            return _warehouseRepository.GetWarehouses();
        }

        public WarehouseEntity GetWarehouse(Guid id)
        {
            return _warehouseRepository.GetWarehouse(id);
        }

        public Task<int> AddWarehouse(WarehouseEntity warehouseEntity)
        {
            return _warehouseRepository.AddWarehouse(warehouseEntity);
        }

        public Task<int> RemoveWarehouse(Guid id)
        {
            return _warehouseRepository.RemoveWarehouse(id);
        }
    }
}