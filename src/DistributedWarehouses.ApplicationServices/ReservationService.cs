using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Domain.Exceptions;
using DistributedWarehouses.Domain.RetrievalServices;
using DistributedWarehouses.Domain.Services;
using DistributedWarehouses.Domain.Validators;
using DistributedWarehouses.Dto;

namespace DistributedWarehouses.ApplicationServices
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRetrievalService _reservationRetrievalService;
        private readonly IMappingService _mappingService;

        public ReservationService(IReservationRetrievalService reservationRetrievalService, IMappingService mappingService)
        {
            _reservationRetrievalService = reservationRetrievalService;
            _mappingService = mappingService;
        }

        public IEnumerable<ReservationEntity> GetReservations()
        {
            var result = _reservationRetrievalService.GetReservations();

            return result;
        }

        public ReservationEntity GetReservation(Guid id)
        {
            var result = _reservationRetrievalService.GetReservation(id);

            return result;
        }

        public async Task<ReservationIdDto> AddReservationAsync(ReservationInputDto reservationInputDto)
        {
            if (reservationInputDto.Reservation is not null && reservationInputDto.Reservation.Equals(Guid.Empty))
            {
                throw new BadRequestException(
                    "Existing reservation cannot have empty id. Remove parameter if no reference to an existing reservation is needed.");
            }
            reservationInputDto.Reservation ??= Guid.NewGuid();
            var reservationItem = _mappingService.Map<ReservationItemEntity>(reservationInputDto);
            return new ReservationIdDto(await _reservationRetrievalService.ReserveItemInWarehouseAsync(reservationItem));
        }

        public async Task RemoveReservationAsync(Guid id)
        {
            await _reservationRetrievalService.RemoveReservationAsync(id);
        }

        public IEnumerable<ReservationItemEntity> GetReservationItems() => _reservationRetrievalService.GetReservationItems();

        public Task<ReservationItemEntity> GetReservationItemAsync(string item, Guid warehouse, Guid reservation) => _reservationRetrievalService.GetReservationItemAsync(item, warehouse, reservation);


        public Task<ReservationItemEntity> AddReservationItemAsync(ReservationItemEntity invoiceItem) => _reservationRetrievalService.AddReservationItemAsync(invoiceItem);

        public async Task RemoveReservationItemAsync(string item, Guid warehouse, Guid reservation)
        {
            await _reservationRetrievalService.RemoveReservationItemAsync(item, warehouse, reservation);
        }
    }
}