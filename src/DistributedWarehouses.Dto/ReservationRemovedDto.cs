using System.Collections.Generic;

namespace DistributedWarehouses.Dto
{
    public class ReservationRemovedDto
    {
        public int ItemsRemovedQuantity { get; set; }

        public ReservationRemovedDto(int itemsRemovedQuantity)
        {
            ItemsRemovedQuantity = itemsRemovedQuantity;
        }
    }
}
