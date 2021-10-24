using System;
using System.Collections.Generic;

#nullable disable

namespace DistributedWarehouses.Infrastructure.Models
{
    public class Item
    {
        public Item()
        {
            ReservationItems = new HashSet<ReservationItem>();
            WarehouseItems = new HashSet<WarehouseItem>();
        }

        public string Sku { get; set; }
        public string Title { get; set; }

        public virtual ICollection<ReservationItem> ReservationItems { get; set; }
        public virtual ICollection<WarehouseItem> WarehouseItems { get; set; }
    }
}
