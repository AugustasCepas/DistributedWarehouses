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
    }
}