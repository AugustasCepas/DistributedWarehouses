using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Repositories;
using DistributedWarehouses.Dto;
using DistributedWarehouses.Infrastructure.Models;
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

        public ReservationRepository(DistributedWarehousesContext distributedWarehousesContext)
        {
            _distributedWarehousesContext = distributedWarehousesContext;
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

        public int AddReservationItem(ReservationInputDto reservationInputDto, Guid reservationId,
            ItemInWarehousesInfoDto itemInWarehouse)
        {
            var itemsInWarehouse = itemInWarehouse.StoredQuantity - itemInWarehouse.ReservedQuantity;
            var itemsToReserve = reservationInputDto.Quantity;

            var reservationQuantity = itemsInWarehouse > itemsToReserve ? itemsToReserve : itemsInWarehouse;


            _distributedWarehousesContext.ReservationItems.Add(new ReservationItemModel
            {
                Quantity = reservationQuantity,
                Item = reservationInputDto.ItemSku,
                Warehouse = itemInWarehouse.WarehouseId,
                Reservation = reservationId
            });
            return _distributedWarehousesContext.SaveChanges();
        }

        public int RemoveReservation(Guid id)
        {
            _distributedWarehousesContext.Reservations.Remove(
                _distributedWarehousesContext.Find<ReservationModel>(id));
            return _distributedWarehousesContext.SaveChanges();
        }

        public int AddReservation(ReservationEntity reservation)
        {
            _distributedWarehousesContext.Reservations.Add(new ReservationModel
            {
                Id = reservation.Id,
                CreatedAt = reservation.CreatedAt,
                ExpirationTime = reservation.ExpirationTime
            });
            return _distributedWarehousesContext.SaveChanges();
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

        public ReservationItemEntity GetReservationItem(string item, Guid warehouse, Guid reservation)
        {
            return _distributedWarehousesContext.ReservationItems
                .Where(i => i.Item == item && i.Warehouse == warehouse && i.Reservation == reservation)
                .Select(i => new ReservationItemEntity
                {
                    Quantity = i.Quantity,
                    Item = i.Item,
                    Warehouse = i.Warehouse,
                    Reservation = i.Reservation
                }).FirstOrDefault();
        }

        public ReservationItemEntity GetReservationItem(Guid reservation)
        {
            return _distributedWarehousesContext.ReservationItems
                .Where(i => i.Reservation == reservation)
                .Select(i => new ReservationItemEntity
                {
                    Quantity = i.Quantity,
                    Item = i.Item,
                    Warehouse = i.Warehouse,
                    Reservation = i.Reservation
                }).FirstOrDefault();
        }

        public async Task<int> AddReservationItem(ReservationItemEntity reservationItemEntity)
        {
            _distributedWarehousesContext.ReservationItems.Add(new ReservationItemModel
            {
                Quantity = reservationItemEntity.Quantity,
                Item = reservationItemEntity.Item,
                Warehouse = reservationItemEntity.Warehouse
            });
            return await _distributedWarehousesContext.SaveChangesAsync();
        }

        public int RemoveReservationItem(string item, Guid warehouse, Guid reservation)
        {
            _distributedWarehousesContext.ReservationItems.Remove(
                _distributedWarehousesContext.Find<ReservationItemModel>(item, warehouse, reservation));
            return _distributedWarehousesContext.SaveChanges();
        }

        public int RemoveReservationItem(ReservationItemEntity reservationItemEntity)
        {
            ReservationItemModel itemReservation = new ReservationItemModel
            {
                Quantity = reservationItemEntity.Quantity,
                Item = reservationItemEntity.Item,
                Warehouse = reservationItemEntity.Warehouse
            };

            _distributedWarehousesContext.ReservationItems.Remove(itemReservation);
            return _distributedWarehousesContext.SaveChanges();
        }
    }
}