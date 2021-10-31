using System;
using System.Collections.Generic;

#nullable disable

namespace DistributedWarehouses.Infrastructure.Models
{
    public partial class InvoiceItem
    {
        public string Item { get; set; }
        public int Quantity { get; set; }
        public Guid Warehouse { get; set; }
        public Guid Invoice { get; set; }

        public virtual Invoice InvoiceNavigation { get; set; }
        public virtual Item ItemNavigation { get; set; }
        public virtual Warehouse WarehouseNavigation { get; set; }
    }
}
