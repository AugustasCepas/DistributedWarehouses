using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Domain.Repositories;
using DistributedWarehouses.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DistributedWarehouses.Infrastructure.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly DistributedWarehousesContext _distributedWarehousesContext;
        private readonly IMapper _mapper;

        public ReservationRepository(DistributedWarehousesContext distributedWarehousesContext, IMapper mapper)
        {
            _distributedWarehousesContext = distributedWarehousesContext;
            _mapper = mapper;
        }

        public IDbContextTransaction GetTransaction()
        {
            return _distributedWarehousesContext.Database.BeginTransaction();
        }

        public ReservationEntity GetReservation(Guid id)
        {
            return _mapper.ProjectTo<ReservationEntity>(_distributedWarehousesContext.Reservations).FirstOrDefault(i => i.Id == id);
        }

        public async Task<ReservationItemEntity> AddReservationItemAsync(ReservationItemEntity reservationItem)
        {
            await _distributedWarehousesContext.ReservationItems
                .Upsert(_mapper.Map<ReservationItem>(reservationItem))
                .On(ri => new {ri.Reservation, ri.Item, ri.Warehouse}).WhenMatched(ri => new ReservationItem
                {
                    Quantity = reservationItem.Quantity
                }).RunAsync(CancellationToken.None);
            return reservationItem;
        }

        public async Task RemoveReservationAsync(Guid id)
        {
            _distributedWarehousesContext.Reservations.Remove(
                _distributedWarehousesContext.Find<Reservation>(id));
            await _distributedWarehousesContext.SaveChangesAsync();
        }

        public async Task<ReservationEntity> AddReservationAsync(ReservationEntity reservation)
        {
            var reservationToAdd = _mapper.Map<Reservation>(reservation);
            await _distributedWarehousesContext.Reservations.Upsert(reservationToAdd).On(r => r.Id)
                .WhenMatched(r => new Reservation
                    {CreatedAt = reservationToAdd.CreatedAt, ExpirationTime = reservationToAdd.ExpirationTime})
                .RunAsync(CancellationToken.None);
            return reservation;
        }

        public IEnumerable<ReservationItemEntity> GetReservationItems(Guid reservation)
        {
            return _mapper.ProjectTo<ReservationItemEntity>(_distributedWarehousesContext
                .ReservationItems).Where(ri => ri.Reservation.Equals(reservation)).AsEnumerable();
        }

        public IEnumerable<ReservationItemEntity> GetReservationItems(Guid reservation, string sku)
        {
            return _mapper.ProjectTo<ReservationItemEntity>(_distributedWarehousesContext.ReservationItems)
                .Where(ri => ri.Reservation.Equals(reservation) && ri.Item.Equals(sku)).AsEnumerable();
        }

        public async Task<int> RemoveReservationItem(IEnumerable<ReservationItemEntity> reservationItems)
        {
            var reservationItemsRemoved = reservationItems.Sum(ri => ri.Quantity);
                _distributedWarehousesContext.ReservationItems.RemoveRange(_mapper.Map<IEnumerable<ReservationItem>>(reservationItems));
            await _distributedWarehousesContext.SaveChangesAsync();

            return reservationItemsRemoved;
        }

        public Task<bool> ExistsAsync<T>(T id)
        {
            return _distributedWarehousesContext.Reservations.AnyAsync(r => r.Id.Equals(id));
        }

        public async Task Add<T>(T entity) where T : DistributableItemEntity
        {
            await AddReservationItemAsync(entity as ReservationItemEntity);
        }
    }
}