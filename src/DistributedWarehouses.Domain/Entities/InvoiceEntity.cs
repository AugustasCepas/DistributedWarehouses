using System;

namespace DistributedWarehouses.Domain.Entities
{
    public class InvoiceEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}