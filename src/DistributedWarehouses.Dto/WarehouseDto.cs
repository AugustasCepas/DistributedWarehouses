using System;

namespace DistributedWarehouses.Dto
{
    public class WarehouseDto
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Address { get; set; } = string.Empty;
        public int Capacity { get; set; } = 0;
        public int StoredQuantity { get; set; } = 0;
        public int ReservedQuantity { get; set; } = 0;
        public int FreeQuantity { get; set; } = 0;

    }
}
