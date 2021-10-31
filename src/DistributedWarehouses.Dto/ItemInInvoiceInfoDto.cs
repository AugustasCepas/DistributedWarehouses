using System;

namespace DistributedWarehouses.Dto
{
    public class ItemInInvoiceInfoDto
    {
        public string ItemId { get; set; }
        public int PurchasedQuantity { get; set; }
        public Guid WarehouseId { get; set; }
    }
}