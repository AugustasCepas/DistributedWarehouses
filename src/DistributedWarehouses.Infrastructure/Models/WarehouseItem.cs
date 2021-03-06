using System;

#nullable disable

namespace DistributedWarehouses.Infrastructure.Models
{
    public partial class WarehouseItem
    {
        public int Quantity { get; set; }
        public string Item { get; set; }
        public Guid Warehouse { get; set; }

        public virtual Item ItemNavigation { get; set; }
        public virtual Warehouse WarehouseNavigation { get; set; }
    }
}
