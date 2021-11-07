using System;

namespace DistributedWarehouses.Dto
{
    public class ReservationInputDto
    {
        public string ItemSku { get; set; }
        public int Quantity { get; set; }
        public Guid? Reservation { get; set; }
    }
}