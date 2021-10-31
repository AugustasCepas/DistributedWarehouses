﻿using System;

namespace DistributedWarehouses.Domain.Entities
{
    public class WarehouseItemEntity
    {
        public int Quantity { get; set; }

        public string Item { get; set; }

        public Guid Warehouse { get; set; }
    }
}