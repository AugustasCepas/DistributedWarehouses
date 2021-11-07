using System;
using DistributedWarehouses.Dto;
using Swashbuckle.AspNetCore.Filters;

namespace DistributedWarehouses.Api.Swagger
{
    public class ItemSellDtoExample : IExamplesProvider<ItemSellDto>
    {
        public ItemSellDto GetExamples()
        {
            return new ItemSellDto{SKU = "item sku", Quantity = 1, ReservationId = Guid.Empty, InvoiceId = Guid.Empty};
        }
    }
}