using System;
using System.Collections.Generic;

#nullable disable

namespace DistributedWarehouses.Infrastructure.Models
{
    public class Reservation
    {
        public Reservation()
        {
            ReservationItems = new HashSet<ReservationItem>();
        }

        public DateTime? CreatedAt { get; set; }
        public DateTime? ExpirationTime { get; set; }
        public Guid Id { get; set; }

        public virtual ICollection<ReservationItem> ReservationItems { get; set; }
    }
}
