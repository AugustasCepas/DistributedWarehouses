using System;
using System.Collections.Generic;

namespace DistributedWarehouses.Dto
{
    public class ItemDto
    {
        public string SKU { get; set; }
        public string Title { get; set; }
        public IEnumerable<ItemInWarehousesInfoDto> InWarehouses { get; set; } = new List<ItemInWarehousesInfoDto>();
    }
}
