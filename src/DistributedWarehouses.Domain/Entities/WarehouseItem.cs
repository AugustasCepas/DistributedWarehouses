using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedWarehouses.Domain.Entities
{
    class WarehouseItem
    {
        public Guid Id { get; set; }

        public int Quantity { get; set; }

        public Item Item { get; set; }

        public Warehouse Warehouse { get; set; }
    }
}