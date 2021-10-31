using System;

namespace DistributedWarehouses.Dto
{
    public class WarehouseDto
    {
        public Guid Id { get; set; }
        public string Address { get; set; }
        public int Capacity { get; set; }
        public int StoredQuantity { get; set; }
        public int ReservedQuantity { get; set; }
        public int FreeQuantity { get; set; }

    }
}
