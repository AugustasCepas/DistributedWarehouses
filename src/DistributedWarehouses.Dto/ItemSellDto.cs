using System;

namespace DistributedWarehouses.Dto
{
    public class ItemSellDto
    {
        public string? SKU { get; set; }
        public int? Quantity { get; set; } 
        public Guid? ReservationId { get; set; }
        public Guid? InvoiceId { get; set; }
    }
}
