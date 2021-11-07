using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Domain.Exceptions;
using DistributedWarehouses.Domain.Repositories;
using DistributedWarehouses.Domain.Services;
using DistributedWarehouses.DomainServices;
using DistributedWarehouses.Dto;

namespace DistributedWarehouses.ApplicationServices
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IMappingService _mappingService;

        public ReservationService(IReservationRepository reservationRepository, IWarehouseRepository warehouseRepository, IMappingService mappingService)
        {
            _reservationRepository = reservationRepository;
            _warehouseRepository = warehouseRepository;
            _mappingService = mappingService;
        }

        public IEnumerable<ReservationEntity> GetReservations()
        {
            var result = _reservationRepository.GetReservations();

            return result;
        }

        public ReservationEntity GetReservation(Guid id)
        {
            var result = _reservationRepository.GetReservation(id);

            return result;
        }

        public async Task<IdDto> AddReservationAsync(ReservationInputDto reservationInputDto)
        {
            if (reservationInputDto.Reservation is not null && reservationInputDto.Reservation.Equals(Guid.Empty))
            {
                throw new BadRequestException(
                    "Existing reservation cannot have empty id. Remove parameter if no reference to an existing reservation is needed.");
            }
            reservationInputDto.Reservation ??= Guid.NewGuid();
            var reservationItem = _mappingService.Map<ReservationItemEntity>(reservationInputDto);
            return new IdDto(await ReserveItemInWarehouseAsync(reservationItem));
        }

        public async Task RemoveReservationAsync(Guid id)
        {
            await _reservationRepository.RemoveReservationAsync(id);
        }

        public IEnumerable<ReservationItemEntity> GetReservationItems() => _reservationRepository.GetReservationItems();
        public IEnumerable<ReservationItemEntity> GetReservationItemsByReservation(Guid reservationId)
        {
            return  _reservationRepository.GetReservationItems(reservationId);
        }

        public Task<ReservationItemEntity> GetReservationItemAsync(string item, Guid warehouse, Guid reservation) => _reservationRepository.GetReservationItemAsync(item, warehouse, reservation);


        public Task<ReservationItemEntity> AddReservationItemAsync(ReservationItemEntity invoiceItem) => _reservationRepository.AddReservationItemAsync(invoiceItem);

        public async Task RemoveReservationItemAsync(string item, Guid warehouse, Guid reservation)
        {
            var result = _reservationRepository.RemoveReservationItem(item, warehouse, reservation);

            if (!_reservationRepository.GetReservationItems(reservation).Any())
            {
                await _reservationRepository.RemoveReservationAsync(reservation);
            }
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
        private async Task<IEnumerable<(Guid, int)>> AddReservationItemsToWarehousesAsync(ReservationItemEntity reservation)
        {
            var warehouses = await new DistributionService(reservation, _warehouseRepository, _reservationRepository, nameof(WarehouseInformation.AvailableItemQuantity)).Distribute();
            return warehouses;
        }
        private Task<ReservationEntity> AddReservationAsync(Guid reservationId)
        {
            return _reservationRepository.AddReservationAsync(new ReservationEntity(reservationId));
        }
    }
}