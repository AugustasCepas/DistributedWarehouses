using System;
using System.Collections.Generic;

#nullable disable

namespace DistributedWarehouses.Infrastructure.Models
{
    public partial class Warehouse
    {
        public Warehouse()
        {
            InvoiceItems = new HashSet<InvoiceItem>();
            ReservationItems = new HashSet<ReservationItem>();
            WarehouseItems = new HashSet<WarehouseItem>();
        }

        public Guid Id { get; set; }
        public string Address { get; set; }
        public int Capacity { get; set; }

        public virtual ICollection<InvoiceItem> InvoiceItems { get; set; }
        public virtual ICollection<ReservationItem> ReservationItems { get; set; }
        public virtual ICollection<WarehouseItem> WarehouseItems { get; set; }
    }
}
