using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;

namespace DistributedWarehouses.ApplicationServices
{
    public interface IReservationService
    {
        IEnumerable<ReservationEntity> GetReservations();
        ReservationEntity GetReservation(Guid Id);
        Task<int> AddReservation(ReservationEntity reservation);
        Task<int> RemoveReservation(Guid Id);
    }
}