using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DistributedWarehouses.Domain;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Domain.Repositories;
using DistributedWarehouses.Domain.RetrievalServices;
using DistributedWarehouses.Dto;
using DistributedWarehouses.Infrastructure.Models;

namespace DistributedWarehouses.DomainServices
{
    public class WarehouseRetrievalService : IWarehouseRetrievalService
    {
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IReservationRepository _reservationRepository;

        public WarehouseRetrievalService(IWarehouseRepository warehouseRepository, IInvoiceRepository invoiceRepository,
            IReservationRepository reservationRepository)
        {
            _warehouseRepository = warehouseRepository;
            _invoiceRepository = invoiceRepository;
            _warehouseRepository = warehouseRepository;
            _reservationRepository = reservationRepository;
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

        public async Task<int> SellWarehouseItem(ItemSellDto dto)
        {
            await _warehouseRepository.UpdateWarehouseItemQuantity(dto.SKU, dto.WarehouseId, dto.Quantity);

            if (GetWarehouseItem(dto.SKU, dto.WarehouseId).Quantity == 0)
            {
                await _warehouseRepository.RemoveWarehouseItem(dto.SKU, dto.WarehouseId);
            }

            if (dto.ReservationId != Guid.Empty && dto.ReservationId != null)
            {
                ReservationItemEntity itemReservation;
                if ((itemReservation = await _reservationRepository.GetReservationItemAsync(dto.SKU, dto.WarehouseId, (Guid)dto.ReservationId)) != null)
                {
                    _reservationRepository.RemoveReservationItem(itemReservation);
                }
            }

            var invoice = new InvoiceEntity();
            var result = await _invoiceRepository.AddInvoice(invoice);

            var invoiceItem = new InvoiceItemEntity()
            {
                Item = dto.SKU,
                Quantity = dto.Quantity,
                Warehouse = dto.WarehouseId,
                Invoice = invoice.Id
            };

            await _invoiceRepository.AddInvoiceItem(invoiceItem);

            return result;
        }
    }
}