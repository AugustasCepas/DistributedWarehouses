using System;

namespace DistributedWarehouses.Dto
{
    public class ReservationIdDto
    {
        public ReservationIdDto(string reservationId)
        {
            ReservationId = Guid.Parse(reservationId);
        }

        public ReservationIdDto(Guid reservationId)
        {
            ReservationId = reservationId;
        }

        public Guid ReservationId { get; }
    }
}