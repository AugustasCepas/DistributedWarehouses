using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Domain.Repositories;
using DistributedWarehouses.Domain.Services;
using DistributedWarehouses.Dto;

namespace DistributedWarehouses.ApplicationServices
{
    public class WarehouseService : IWarehouseService
    {
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IReservationRepository _reservationRepository;

        public WarehouseService(IWarehouseRepository warehouseRepository, IInvoiceRepository invoiceRepository,
            IReservationRepository reservationRepository)
        {
            _warehouseRepository = warehouseRepository;
            _invoiceRepository = invoiceRepository;
            _warehouseRepository = warehouseRepository;
            _reservationRepository = reservationRepository;
        }

        public IEnumerable<WarehouseEntity> GetWarehouses()
        {
            var result = _warehouseRepository.GetWarehouses();

            return result;
        }

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

        public Task<WarehouseItemEntity> AddWarehouseItem(WarehouseItemEntity warehouseItemEntity)
        {
            if (_warehouseRepository.GetWarehouseItem(warehouseItemEntity.Item, warehouseItemEntity.Warehouse) != null)
            {
                return Task.FromResult<WarehouseItemEntity>(null);
            }

            return _warehouseRepository.AddWarehouseItem(warehouseItemEntity);
        }
    }
}