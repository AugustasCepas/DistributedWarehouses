using System;

namespace DistributedWarehouses.Domain.Entities
{
    public class DistributableItemEntity
    {
        public string Item { get; set; }
        public int Quantity { get; set; }
        public Guid Warehouse { get; set; }
    }
}
