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
            var warehouses = (await new DistributionService(reservation, _warehouseRepository, _reservationRepository).Distribute());
            return warehouses;
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