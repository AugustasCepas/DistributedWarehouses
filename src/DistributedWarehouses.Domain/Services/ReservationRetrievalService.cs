using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Domain.Repositories;

namespace DistributedWarehouses.Domain.Services
{
    public class ReservationRetrievalService : IReservationRetrievalService
    {
        private readonly IReservationRepository _reservationRepository;

        public ReservationRetrievalService(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public IEnumerable<ReservationEntity> GetReservations()
        {
            return _reservationRepository.GetReservations();
        }

        public ReservationEntity GetReservation(Guid Id)
        {
            return _reservationRepository.GetReservation(Id);
        }

        public Task<int> AddReservation(ReservationEntity reservation)
        {
            return _reservationRepository.AddReservation(reservation);
        }

        public Task<int> RemoveReservation(Guid Id)
        {
            return _reservationRepository.RemoveReservation(Id);
        }
    }
}