using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace DistributedWarehouses.Domain.Repositories
{
    public interface IReservationRepository : IRepository
    {
        IDbContextTransaction GetTransaction();
        ReservationEntity GetReservation(Guid id);
        Task<ReservationEntity> AddReservationAsync(ReservationEntity reservation);
        
        Task<ReservationItemEntity> AddReservationItemAsync(ReservationItemEntity reservationItem);

        Task RemoveReservationAsync(Guid id);

        // Reservation Item
        IEnumerable<ReservationItemEntity> GetReservationItems(Guid reservation);
        IEnumerable<ReservationItemEntity> GetReservationItems(Guid reservation, string sku);
        Task<int> RemoveReservationItem(IEnumerable<ReservationItemEntity> reservationItems);
    }
}