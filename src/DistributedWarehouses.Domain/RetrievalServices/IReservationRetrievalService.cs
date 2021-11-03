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
        Task<Guid> ReserveItemInWarehouseAsync(ReservationItemEntity reservationInputDto);
        Task RemoveReservationAsync(Guid id);


        // Reservation Item
        IEnumerable<ReservationItemEntity> GetReservationItems();
        Task<ReservationItemEntity> GetReservationItemAsync(string item, Guid warehouse, Guid reservation);
        Task<ReservationItemEntity> AddReservationItemAsync(ReservationItemEntity reservationItem);
        Task<int> RemoveReservationItemAsync(string item, Guid warehouse, Guid reservation);
    }
}