﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedWarehouses.Domain.Entities
{
    public class ReservationItemEntity : DistributableItemEntity
    {
        public Guid Reservation { get; set; }
    }
}