using System;

namespace DistributedWarehouses.Domain.Entities
{
    public class WarehouseEntity
    {
        public Guid Id { get; set; }

        public string Address { get; set; }

        /// <summary>
        /// WarehouseEntity capacity in items
        /// </summary>
        public int Capacity { get; set; }

        public int FreeQuantity { get; set; }
        public int StoredItemQuantity { get; set; }
        public int AvailableItemQuantity { get; set; }
    }
}