using System;
using System.Collections.Generic;

namespace DistributedWarehouses.Dto
{
    public class ItemDto
    {
        public string SKU { get; set; } = String.Empty;
        public string Title { get; set; } = String.Empty;
        public IEnumerable<ItemInWarehousesInfoDto> InWarehouses { get; set; } = new List<ItemInWarehousesInfoDto>();
    }
}
