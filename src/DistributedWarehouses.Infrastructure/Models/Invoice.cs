using System;
using System.Collections.Generic;

#nullable disable

namespace DistributedWarehouses.Infrastructure.Models
{
    public partial class Invoice
    {
        public Invoice()
        {
            InvoiceItems = new HashSet<InvoiceItem>();
        }

        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<InvoiceItem> InvoiceItems { get; set; }
    }
}
