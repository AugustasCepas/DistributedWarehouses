﻿using System;

namespace DistributedWarehouses.Dto
{
    public class ItemInWarehousesInfoDto
    {
        public Guid WarehouseId { get; set; }
        public int StoredQuantity { get; set; } = 0;
        public int ReservedQuantity { get; set; } = 0;
    }
}