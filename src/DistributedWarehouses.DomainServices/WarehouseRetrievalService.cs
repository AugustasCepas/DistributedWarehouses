using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DistributedWarehouses.Domain;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Domain.RetrievalServices;
using DistributedWarehouses.Dto;

namespace DistributedWarehouses.DomainServices
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

        /// <summary>
        /// Return info of one warehouse
        /// How many goods are stored
        /// How many goods are reserved
        /// How much free space available
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WarehouseDto GetWarehouseInfo(Guid id)
        {
            WarehouseEntity warehouse = _warehouseRepository.GetWarehouse(id);
            WarehouseDto result = _warehouseRepository.GetWarehouseInfo(id);

            result.Id = warehouse.Id;
            result.Address = warehouse.Address;
            result.Capacity = warehouse.Capacity;
            result.FreeQuantity = warehouse.Capacity - result.StoredQuantity;

            return result;
        }

        public Task<int> AddWarehouse(WarehouseEntity warehouseEntity)
        {
            return _warehouseRepository.AddWarehouse(warehouseEntity);
        }

        public Task<int> RemoveWarehouse(Guid id)
        {
            return _warehouseRepository.RemoveWarehouse(id);
        }

        //Warehouse Item
        public IEnumerable<WarehouseItemEntity> GetWarehouseItems()
        {
            return _warehouseRepository.GetWarehouseItems();
        }

        public WarehouseItemEntity GetWarehouseItem(string item, Guid warehouse)
        {
            return _warehouseRepository.GetWarehouseItem(item, warehouse);
        }

        public Task<int> AddWarehouseItem(WarehouseItemEntity warehouseItemEntity)
        {
            if (_warehouseRepository.GetWarehouseItem(warehouseItemEntity.Item, warehouseItemEntity.Warehouse) != null)
            {
                return Task.FromResult(0);
            }
            return _warehouseRepository.AddWarehouseItem(warehouseItemEntity);
        }

        public Task<int> RemoveWarehouseItem(string item, Guid warehouse)
        {
            return _warehouseRepository.RemoveWarehouseItem(item, warehouse);
        }
    }
}