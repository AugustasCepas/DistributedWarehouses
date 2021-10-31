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

        public string Item { get; set; }

        public Guid Warehouse { get; set; }

        public Guid Reservation { get; set; }
    }
}