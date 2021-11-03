using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Dto;
using Microsoft.EntityFrameworkCore.Storage;

namespace DistributedWarehouses.Domain.Repositories
{
    public interface IReservationRepository : IRepository
    {
        IDbContextTransaction GetTransaction();
        IEnumerable<ReservationEntity> GetReservations();
        ReservationEntity GetReservation(Guid id);
        Task<ReservationEntity> AddReservationAsync(ReservationEntity reservation);
        
        Task<ReservationItemEntity> AddReservationItemAsync(ReservationItemEntity reservationItem);

        Task RemoveReservationAsync(Guid id);

        // Reservation Item
        IEnumerable<ReservationItemEntity> GetReservationItems();
        Task<ReservationItemEntity> GetReservationItemAsync(string item, Guid warehouse, Guid reservation);
        Task<ReservationItemEntity> GetReservationItemAsync(Guid reservation);
        int RemoveReservationItem(string item, Guid warehouse, Guid reservation);
        int RemoveReservationItem(ReservationItemEntity reservationItemEntity);
    }
}