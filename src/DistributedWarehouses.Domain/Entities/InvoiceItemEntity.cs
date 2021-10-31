using System;

namespace DistributedWarehouses.Domain.Entities
{
    public class InvoiceItemEntity
    {
        public string Item { get; set; }
        public int Quantity { get; set; }
        public Guid Warehouse { get; set; }
        public Guid Invoice { get; set; }
    }
}