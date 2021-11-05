using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedWarehouses.Domain.Entities
{
    public class DistributableItemEntity
    {
        public string Item { get; set; }
        public Guid Warehouse { get; set; }
        public int Quantity { get; set; }
    }
}
