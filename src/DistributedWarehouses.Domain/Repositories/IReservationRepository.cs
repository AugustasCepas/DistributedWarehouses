using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Dto;
using Microsoft.EntityFrameworkCore.Storage;

namespace DistributedWarehouses.Domain.Repositories
{
    public interface IReservationRepository
    {
        IDbContextTransaction GetTransaction();
        IEnumerable<ReservationEntity> GetReservations();
        ReservationEntity GetReservation(Guid id);
        int AddReservation(ReservationEntity reservation);

        int AddReservationItem(ReservationInputDto reservationInputDto, Guid reservationId,
            ItemInWarehousesInfoDto itemInWarehouses);

        int RemoveReservation(Guid id);

        // Reservation Item
        IEnumerable<ReservationItemEntity> GetReservationItems();
        ReservationItemEntity GetReservationItem(string item, Guid warehouse, Guid reservation);
        ReservationItemEntity GetReservationItem(Guid reservation);
        Task<int> AddReservationItem(ReservationItemEntity reservationItemEntity);
        int RemoveReservationItem(string item, Guid warehouse, Guid reservation);
        int RemoveReservationItem(ReservationItemEntity reservationItemEntity);
    }
}