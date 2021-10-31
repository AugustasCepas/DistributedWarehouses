﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Domain.Repositories;
using DistributedWarehouses.Domain.RetrievalServices;
using DistributedWarehouses.Dto;

namespace DistributedWarehouses.DomainServices
{
    public class ReservationRetrievalService : IReservationRetrievalService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IItemRepository _itemRepository;

        public ReservationRetrievalService(IReservationRepository reservationRepository, IItemRepository itemRepository)
        {
            _reservationRepository = reservationRepository;
            _itemRepository = itemRepository;
        }

        public IEnumerable<ReservationEntity> GetReservations()
        {
            return _reservationRepository.GetReservations();
        }

        public ReservationEntity GetReservation(Guid id)
        {
            return _reservationRepository.GetReservation(id);
        }

        public int AddReservation(ReservationInputDto reservationInputDto)
        {
            using (var transaction = _reservationRepository.GetTransaction())
            {
                try
                {
                    var reservation = new ReservationEntity();
                    _reservationRepository.AddReservation(reservation);

                    var itemInWarehouses =
                        _itemRepository.GetItemInWarehousesInfo(reservationInputDto.ItemSku)
                            .OrderByDescending(i => i.StoredQuantity - i.ReservedQuantity);

                    var itemsToReserve = reservationInputDto.Quantity;

                    foreach (var warehouse in itemInWarehouses)
                    {
                        if (itemsToReserve <= 0) break;

                        var itemsInWarehouse = warehouse.StoredQuantity - warehouse.ReservedQuantity;

                        _reservationRepository.AddReservationItem(reservationInputDto, reservation, warehouse);
                        itemsToReserve -= itemsInWarehouse;
                    }

                    transaction.Commit();
                    return 1;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    // Console.WriteLine(e);
                    throw;
                }
            }
        }

        public int RemoveReservation(Guid id)
        {
            return _reservationRepository.RemoveReservation(id);
        }


        // Reservation Item
        public IEnumerable<ReservationItemEntity> GetReservationItems()
        {
            return _reservationRepository.GetReservationItems();
        }

        public ReservationItemEntity GetReservationItem(string item, Guid warehouse, Guid reservation)
        {
            return _reservationRepository.GetReservationItem(item, warehouse, reservation);
        }

        public Task<int> AddReservationItem(ReservationItemEntity reservationItem)
        {
            return _reservationRepository.AddReservationItem(reservationItem);
        }

        public int RemoveReservationItem(string item, Guid warehouse, Guid reservation)
        {
            try
            {
                var result = _reservationRepository.RemoveReservationItem(item, warehouse, reservation);

                if (_reservationRepository.GetReservationItem(reservation) == null)
                {
                    _reservationRepository.RemoveReservation(reservation);
                }

                return result;
            }
            catch (Exception e)
            {
                // Console.WriteLine(e);
                throw;
            }
        }
    }
}