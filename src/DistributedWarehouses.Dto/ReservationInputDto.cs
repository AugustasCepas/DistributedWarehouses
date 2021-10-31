using System;
using System.Collections.Generic;

namespace DistributedWarehouses.Dto
{
    public class ReservationInputDto
    {
        public string ItemSku { get; set; } = string.Empty;
        public int Quantity { get; set; } = 0;
    }
}