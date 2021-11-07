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
        Task<IdDto> AddReservationAsync(ReservationInputDto reservationInputDto);
        Task RemoveReservationAsync(Guid id);


        // Reservation Item
        IEnumerable<ReservationItemEntity> GetReservationItems();
        IEnumerable<ReservationItemEntity> GetReservationItemsByReservation(Guid reservationId);
        Task<ReservationItemEntity> AddReservationItemAsync(ReservationItemEntity invoiceItem);
        Task RemoveReservationItemAsync(string item, Guid warehouse, Guid reservation);
    }
}