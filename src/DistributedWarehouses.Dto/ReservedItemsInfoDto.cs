using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedWarehouses.Dto
{
    class ReservedItemsInfoDto
    {
        public Guid WarehouseId { get; set; }
        public int Quantity { get; set; }
    }
}
