using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Dto;

namespace DistributedWarehouses.Domain.RetrievalServices
{
    public interface IReservationRetrievalService
    {
        IEnumerable<ReservationEntity> GetReservations();
        ReservationEntity GetReservation(Guid id);
        Guid ReserveItemInWarehouse(ReservationInputDto reservationInputDto);
        int RemoveReservation(Guid id);


        // Reservation Item
        IEnumerable<ReservationItemEntity> GetReservationItems();
        ReservationItemEntity GetReservationItem(string item, Guid warehouse, Guid reservation);
        Task<int> AddReservationItem(ReservationItemEntity reservationItem);
        int RemoveReservationItem(string item, Guid warehouse, Guid reservation);
    }
}