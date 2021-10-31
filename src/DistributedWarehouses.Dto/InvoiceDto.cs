using System;
using System.Collections.Generic;

namespace DistributedWarehouses.Dto
{
    public class InvoiceDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public IEnumerable<ItemInInvoiceInfoDto> Items { get; set; } = new List<ItemInInvoiceInfoDto>();
    }
}