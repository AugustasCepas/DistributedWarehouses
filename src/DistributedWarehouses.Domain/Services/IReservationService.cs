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
        int AddReservation(ReservationInputDto reservationInputDto);
        int RemoveReservation(Guid id);


        // Reservation Item
        IEnumerable<ReservationItemEntity> GetReservationItems();
        Task<int> AddReservationItem(ReservationItemEntity invoiceItem);
        int RemoveReservationItem(string item, Guid warehouse, Guid reservation);
    }
}