using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedWarehouses.Domain.Entities
{
    public class ReservationItemEntity
    {
        public int Quantity { get; set; }

        public ItemEntity Item { get; set; }

        public WarehouseEntity WarehouseEntity { get; set; }

        public ReservationEntity ReservationEntity { get; set; }
    }
}