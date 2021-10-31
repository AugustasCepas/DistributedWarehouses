using System;

namespace DistributedWarehouses.Dto
{
    public class ItemInWarehousesInfoDto
    {
        public Guid WarehouseId { get; set; }
        public int StoredQuantity { get; set; }
        public int ReservedQuantity { get; set; }
    }
}