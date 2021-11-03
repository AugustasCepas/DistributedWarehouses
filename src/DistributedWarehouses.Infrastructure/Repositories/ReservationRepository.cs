using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DistributedWarehouses.Domain.Repositories;
using DistributedWarehouses.Dto;
using DistributedWarehouses.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ReservationEntity = DistributedWarehouses.Domain.Entities.ReservationEntity;
using ReservationModel = DistributedWarehouses.Infrastructure.Models.Reservation;
using ReservationItemEntity = DistributedWarehouses.Domain.Entities.ReservationItemEntity;
using ReservationItemModel = DistributedWarehouses.Infrastructure.Models.ReservationItem;

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

        public IEnumerable<ReservationEntity> GetReservations()
        {
            return _distributedWarehousesContext.Reservations.Select(i => new ReservationEntity
            {
                Id = i.Id,
                CreatedAt = i.CreatedAt,
                ExpirationTime = i.ExpirationTime
            }).AsEnumerable();
        }

        public ReservationEntity GetReservation(Guid id)
        {
            return _distributedWarehousesContext.Reservations
                .Where(i => i.Id == id)
                .Select(i => new ReservationEntity
                {
                    Id = i.Id,
                    CreatedAt = i.CreatedAt,
                    ExpirationTime = i.ExpirationTime
                }).FirstOrDefault();
        }

        // public int AddReservationItem(ReservationInputDto reservationInputDto, Guid reservationId,ItemInWarehousesInfoDto itemInWarehouse)
        public async Task<ReservationItemEntity> AddReservationItemAsync(ReservationItemEntity reservationItem)
        {
            await _distributedWarehousesContext.ReservationItems
                .Upsert(_mapper.Map<ReservationItemModel>(reservationItem))
                .On(ri => new { ri.Reservation, ri.Item, ri.Warehouse }).WhenMatched(ri => new ReservationItemModel
                {
                    Quantity = reservationItem.Quantity
                }).RunAsync(CancellationToken.None);
            return reservationItem;
        }

        public async Task RemoveReservationAsync(Guid id)
        {
            _distributedWarehousesContext.Reservations.Remove(
                _distributedWarehousesContext.Find<ReservationModel>(id));
            await _distributedWarehousesContext.SaveChangesAsync();
        }

        public async Task<ReservationEntity> AddReservationAsync(ReservationEntity reservation)
        {
            var reservationToAdd = _mapper.Map<Reservation>(reservation);
            await _distributedWarehousesContext.Reservations.Upsert(reservationToAdd).On(r => r.Id)
                .WhenMatched(r => reservationToAdd).RunAsync(CancellationToken.None);
            return reservation;
        }

        public IEnumerable<ReservationItemEntity> GetReservationItems()
        {
            return _distributedWarehousesContext.ReservationItems.Select(i => new ReservationItemEntity
            {
                Quantity = i.Quantity,
                Item = i.Item,
                Warehouse = i.Warehouse,
                Reservation = i.Reservation
            }).AsEnumerable();
        }

        public Task<ReservationItemEntity> GetReservationItemAsync(string item, Guid warehouse, Guid reservation)
        {
            return _mapper
                .ProjectTo<ReservationItemEntity>(_distributedWarehousesContext.ReservationItems)
                .FirstOrDefaultAsync(i => i.Item == item && i.Warehouse == warehouse && i.Reservation == reservation);
        }

        public Task<ReservationItemEntity> GetReservationItemAsync(Guid reservation)
        {
            return _mapper.ProjectTo<ReservationItemEntity>(_distributedWarehousesContext.ReservationItems)
                .FirstOrDefaultAsync(i => i.Reservation == reservation);
        }

        public int RemoveReservationItem(string item, Guid warehouse, Guid reservation)
        {
            _distributedWarehousesContext.ReservationItems.Remove(
                _distributedWarehousesContext.Find<ReservationItemModel>(item, warehouse, reservation));
            return _distributedWarehousesContext.SaveChanges();
        }

        public int RemoveReservationItem(ReservationItemEntity reservationItemEntity)
        {
            _distributedWarehousesContext.ReservationItems.Remove(
                _mapper.Map<ReservationItemModel>(reservationItemEntity));
            return _distributedWarehousesContext.SaveChanges();
        }

        public Task<bool> ExistsAsync<T>(T id)
        {
            return _distributedWarehousesContext.Reservations.AnyAsync(r => r.Id.Equals(id));
        }
    }
}