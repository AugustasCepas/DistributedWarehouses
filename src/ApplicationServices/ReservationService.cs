using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Domain.Services;

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
            return _reservationRetrievalRepository.GetReservations();
        }

        public ReservationEntity GetReservation(Guid Id)
        {
            return _reservationRetrievalRepository.GetReservation(Id);
        }

        public Task<int> AddReservation(ReservationEntity reservation)
        {
            return _reservationRetrievalRepository.AddReservation(reservation);
        }

        public Task<int> RemoveReservation(Guid Id)
        {
            return _reservationRetrievalRepository.RemoveReservation(Id);
        }
    }
}