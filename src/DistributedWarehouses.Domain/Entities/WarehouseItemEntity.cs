using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedWarehouses.Domain.Entities
{
    public class WarehouseItemEntity
    {
        public int Quantity { get; set; }

        public ItemEntity Item { get; set; }

        public WarehouseEntity WarehouseEntity { get; set; }
    }
}