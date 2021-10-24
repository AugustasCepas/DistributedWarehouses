using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedWarehouses.Domain.Entities
{
    class Invoice
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
