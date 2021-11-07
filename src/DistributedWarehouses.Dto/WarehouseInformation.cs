using System;

namespace DistributedWarehouses.Dto
{
    public class WarehouseInformation
    {
        public Guid Id { get; set; }
        public int Capacity { get; set; }
        public int FreeQuantity { get; set; }
        public int StoredItemQuantity { get; set; }
        public int AvailableItemQuantity { get; set; }
    }
}
