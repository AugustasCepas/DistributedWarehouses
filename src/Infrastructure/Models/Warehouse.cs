using System;
using System.Collections.Generic;

#nullable disable

namespace DistributedWarehouses.Infrastructure.Models
{
    public class Warehouse
    {
        public Warehouse()
        {
            ReservationItems = new HashSet<ReservationItem>();
            WarehouseItems = new HashSet<WarehouseItem>();
        }

        public Guid Id { get; set; }
        public string Address { get; set; }
        public int? Capacity { get; set; }

        public virtual ICollection<ReservationItem> ReservationItems { get; set; }
        public virtual ICollection<WarehouseItem> WarehouseItems { get; set; }
    }
}
