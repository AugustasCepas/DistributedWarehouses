using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Domain.Exceptions;
using DistributedWarehouses.Domain.Repositories;
using DistributedWarehouses.Domain.Resources;
using DistributedWarehouses.Domain.RetrievalServices;
using DistributedWarehouses.Dto;

namespace DistributedWarehouses.DomainServices
{
    public class ReservationRetrievalService : IReservationRetrievalService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IWarehouseRepository _warehouseRepository;

        public ReservationRetrievalService(IReservationRepository reservationRepository, IWarehouseRepository warehouseRepository)
        {
            _reservationRepository = reservationRepository;
            _warehouseRepository = warehouseRepository;
        }

        public IEnumerable<ReservationEntity> GetReservations()
        {
            return _reservationRepository.GetReservations();
        }

        public ReservationEntity GetReservation(Guid id)
        {
            return _reservationRepository.GetReservation(id);
        }

        public async Task<Guid> ReserveItemInWarehouseAsync(ReservationItemEntity reservationItem)
        {
            using (var transaction = _reservationRepository.GetTransaction())
            {
                try
                {
                    await AddReservationAsync(reservationItem.Reservation);
                    await AddReservationItemsToWarehousesAsync(reservationItem);
                    await transaction.CommitAsync();
                }
                catch 
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            return reservationItem.Reservation;
        }

        private async Task<IEnumerable<(Guid,int)>> AddReservationItemsToWarehousesAsync(ReservationItemEntity reservation)
        {
            var list = new List<(Guid,int)>();
            var quantity = reservation.Quantity;
            var (warehouse, freeItems) = await GetWarehouseParams(reservation.Item);
            while (quantity > 0 && freeItems > 0)
            {
                reservation.Warehouse = warehouse;
                reservation.Quantity = freeItems >= quantity ? quantity : freeItems;
                await AddReservationItemAsync(reservation);
                list.Add((warehouse, reservation.Quantity));
                quantity -= reservation.Quantity;
                (warehouse, freeItems) = await GetWarehouseParams(reservation.Item);
            }

            if (quantity>0)
            {
                throw new InsufficientStorageException(quantity);
            }

            return list;
        }

        private async Task<(Guid, int)> GetWarehouseParams(string sku)
        {
            var warehouseItem = await _warehouseRepository.GetLargestWarehouseByFreeItemsQuantity(sku);
            return (warehouseItem.Warehouse, warehouseItem.Quantity);
        }

        private Task<ReservationEntity> AddReservationAsync(Guid reservationId)
        {
            return _reservationRepository.AddReservationAsync(new ReservationEntity(reservationId));
        }

        public async Task RemoveReservationAsync(Guid id)
        {
            await _reservationRepository.RemoveReservationAsync(id);
        }

        public IEnumerable<ReservationItemEntity> GetReservationItems()
        {
            return _reservationRepository.GetReservationItems();
        }

        public Task<ReservationItemEntity> GetReservationItemAsync(string item, Guid warehouse, Guid reservation)
        {
            return _reservationRepository.GetReservationItemAsync(item, warehouse, reservation);
        }

        public async Task<ReservationItemEntity> AddReservationItemAsync(ReservationItemEntity reservationItem)
        {
            return await _reservationRepository.AddReservationItemAsync(reservationItem);
        }

        public async Task<int> RemoveReservationItemAsync(string item, Guid warehouse, Guid reservation)
        {
            var result = _reservationRepository.RemoveReservationItem(item, warehouse, reservation);

            if ( await _reservationRepository.GetReservationItemAsync(reservation) == null)
            {
                await _reservationRepository.RemoveReservationAsync(reservation);
            }

            return result;
        }
    }
}