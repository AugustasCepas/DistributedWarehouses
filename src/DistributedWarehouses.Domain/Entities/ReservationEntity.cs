using System;

namespace DistributedWarehouses.Domain.Entities
{
    public class ReservationEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpirationTime { get; set; }

        public ReservationEntity()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.Now;
            ExpirationTime = DateTime.Now.AddDays(14);
        }
        public ReservationEntity(Guid id)
        {
            Id = id;
            CreatedAt = DateTime.Now;
            ExpirationTime = DateTime.Now.AddDays(14);
        }
    }
}