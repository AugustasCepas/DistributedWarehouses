using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedWarehouses.Dto
{
    public class ItemSellDto
    {
        public string SKU { get; set; }
        public int Quantity { get; set; } 
        public Guid WarehouseId { get; set; }
        public Guid? ReservationId { get; set; }
    }
}
