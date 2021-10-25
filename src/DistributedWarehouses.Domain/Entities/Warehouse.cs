using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedWarehouses.Domain.Entities
{
    public class Warehouse
    {
        public Guid Id { get; set; }

        public string Address { get; set; }

        /// <summary>
        /// Warehouse capacity in items
        /// </summary>
        public int? Capacity { get; set; }
    }
}
