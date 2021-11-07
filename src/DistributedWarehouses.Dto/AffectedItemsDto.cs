using System.Collections.Generic;

namespace DistributedWarehouses.Dto
{
    public class AffectedItemsDto
    {
        public int ItemsQuantity { get; set; }

        public AffectedItemsDto(int itemsQuantity)
        {
            ItemsQuantity = itemsQuantity;
        }
    }
}
