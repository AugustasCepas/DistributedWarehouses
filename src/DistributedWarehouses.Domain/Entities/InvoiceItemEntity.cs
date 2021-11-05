using System;

namespace DistributedWarehouses.Domain.Entities
{
    public class InvoiceItemEntity : DistributableItemEntity
    {
        public Guid Invoice { get; set; }
    }
}