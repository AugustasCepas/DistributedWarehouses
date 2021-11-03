using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Dto;

namespace DistributedWarehouses.Domain.Services
{
    public interface IReservationService
    {
        IEnumerable<ReservationEntity> GetReservations();
        ReservationEntity GetReservation(Guid id);
        Task<ReservationIdDto> AddReservationAsync(ReservationInputDto reservationInputDto);
        Task RemoveReservationAsync(Guid id);


        // Reservation Item
        IEnumerable<ReservationItemEntity> GetReservationItems();
        Task<ReservationItemEntity> AddReservationItemAsync(ReservationItemEntity invoiceItem);
        Task RemoveReservationItemAsync(string item, Guid warehouse, Guid reservation);
    }
}