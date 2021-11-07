using System.Collections.Generic;

#nullable disable

namespace DistributedWarehouses.Infrastructure.Models
{
    public partial class Item
    {
        public Item()
        {
            InvoiceItems = new HashSet<InvoiceItem>();
            ReservationItems = new HashSet<ReservationItem>();
            WarehouseItems = new HashSet<WarehouseItem>();
        }

        public string Sku { get; set; }
        public string Title { get; set; }

        public virtual ICollection<InvoiceItem> InvoiceItems { get; set; }
        public virtual ICollection<ReservationItem> ReservationItems { get; set; }
        public virtual ICollection<WarehouseItem> WarehouseItems { get; set; }
    }
}
