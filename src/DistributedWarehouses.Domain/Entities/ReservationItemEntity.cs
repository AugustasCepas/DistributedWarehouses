using System;

namespace DistributedWarehouses.Domain.Entities
{
    public class ReservationItemEntity : DistributableItemEntity
    {
        public Guid Reservation { get; set; }
    }
}