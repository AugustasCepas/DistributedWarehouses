using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Domain.RetrievalServices;
using DistributedWarehouses.Domain.Services;
using DistributedWarehouses.Dto;

namespace DistributedWarehouses.ApplicationServices
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRetrievalService _reservationRetrievalRepository;

        public ReservationService(IReservationRetrievalService reservationRetrievalRepository)
        {
            _reservationRetrievalRepository = reservationRetrievalRepository;
        }

        public IEnumerable<ReservationEntity> GetReservations()
        {
            var result = _reservationRetrievalRepository.GetReservations();

            return result;
        }

        public ReservationEntity GetReservation(Guid id)
        {
            var result = _reservationRetrievalRepository.GetReservation(id);

            return result;
        }

        public int AddReservation(ReservationInputDto reservationInputDto)
        {
            var result = _reservationRetrievalRepository.AddReservation(reservationInputDto);

            return result;
        }

        public int RemoveReservation(Guid id)
        {
            var result = _reservationRetrievalRepository.RemoveReservation(id);

            return result;
        }

        public IEnumerable<ReservationItemEntity> GetReservationItems()
        {
            var result = _reservationRetrievalRepository.GetReservationItems();

            return result;
        }

        public ReservationItemEntity GetReservationItem(string item, Guid warehouse, Guid reservation)
        {
            var result = _reservationRetrievalRepository.GetReservationItem(item, warehouse, reservation);

            return result;
        }


        public Task<int> AddReservationItem(ReservationItemEntity invoiceItem)
        {
             var result = _reservationRetrievalRepository.AddReservationItem(invoiceItem);
             
             return result;
        }

        public int RemoveReservationItem(string item, Guid warehouse, Guid reservation)
        {
             var result = _reservationRetrievalRepository.RemoveReservationItem(item, warehouse, reservation);

             return result;
        }
    }
}