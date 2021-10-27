using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Repositories;
using DistributedWarehouses.Infrastructure.Models;
using ReservationEntity = DistributedWarehouses.Domain.Entities.ReservationEntity;
using ReservationModel = DistributedWarehouses.Infrastructure.Models.Reservation;

namespace DistributedWarehouses.Infrastructure.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly DistributedWarehousesContext _distributedWarehousesContext;

        public ReservationRepository(DistributedWarehousesContext distributedWarehousesContext)
        {
            _distributedWarehousesContext = distributedWarehousesContext;
        }

        public IEnumerable<ReservationEntity> GetReservations()
        {
            return _distributedWarehousesContext.Reservations.Select(i => new ReservationEntity
            {
                Id = i.Id,
                CreatedAt = i.CreatedAt,
                ExpirationTime = i.ExpirationTime,
            }).AsEnumerable();
        }

        public ReservationEntity GetReservation(Guid Id)
        {
            return _distributedWarehousesContext.Reservations
                .Where(i => i.Id == Id)
                .Select(i => new ReservationEntity
                {
                    Id = i.Id,
                    CreatedAt = i.CreatedAt,
                    ExpirationTime = i.ExpirationTime
                }).FirstOrDefault();
        }

        public Task<int> AddReservation(ReservationEntity reservation)
        {
            _distributedWarehousesContext.Reservations.Add(new ReservationModel
            {
                Id = reservation.Id,
                CreatedAt = reservation.CreatedAt,
                ExpirationTime = reservation.ExpirationTime
            });
            return _distributedWarehousesContext.SaveChangesAsync();
        }

        public async Task<int> RemoveReservation(Guid Id)
        {
            _distributedWarehousesContext.Reservations.Remove(
                await _distributedWarehousesContext.FindAsync<ReservationModel>(Id));
            return await _distributedWarehousesContext.SaveChangesAsync();
        }
    }
}