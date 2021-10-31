using System;

namespace DistributedWarehouses.Dto
{
    public class ItemInInvoiceInfoDto
    {
        public string ItemId { get; set; } = String.Empty;
        public int PurchasedQuantity { get; set; } = 0;
        public Guid WarehouseId { get; set; } = Guid.Empty;
    }
}