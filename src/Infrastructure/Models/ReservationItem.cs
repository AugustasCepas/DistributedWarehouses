﻿using System;
using System.Collections.Generic;

#nullable disable

namespace DistributedWarehouses.Infrastructure.Models
{
    public class ReservationItem
    {
        public int? Quantity { get; set; }
        public string Item { get; set; }
        public Guid Warehouse { get; set; }
        public Guid Reservation { get; set; }

        public virtual Item ItemNavigation { get; set; }
        public virtual Reservation ReservationNavigation { get; set; }
        public virtual Warehouse WarehouseNavigation { get; set; }
    }
}
