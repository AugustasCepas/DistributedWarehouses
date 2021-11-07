using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Dto;

namespace DistributedWarehouses.Domain.Services
{
    public interface IReservationService
    {
        ReservationEntity GetReservation(Guid id);
        Task<IdDto> AddReservationAsync(ReservationInputDto reservationInputDto);
        Task RemoveReservationAsync(Guid id);
        Task<IEnumerable<ReservationItemEntity>> GetReservationItemsByReservation(Guid reservationId);
        Task<ReservationRemovedDto> RemoveReservationItemAsync(string item, Guid reservation);
    }
}