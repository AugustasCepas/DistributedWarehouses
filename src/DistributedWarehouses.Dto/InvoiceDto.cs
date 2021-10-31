using System;
using System.Collections.Generic;

namespace DistributedWarehouses.Dto
{
    public class InvoiceDto
    {
        public Guid Id { get; set; } =Guid.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.MinValue;
        public IEnumerable<ItemInInvoiceInfoDto> Items { get; set; } = new List<ItemInInvoiceInfoDto>();
    }
}