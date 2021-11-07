using System;
using DistributedWarehouses.Dto;
using Swashbuckle.AspNetCore.Filters;

namespace DistributedWarehouses.Api.Swagger
{
    public class ReservationInputDtoExample : IExamplesProvider<ReservationInputDto>
    {
        public ReservationInputDto GetExamples()
        {
            return new ReservationInputDto { ItemSku = "item sku", Quantity = 1, Reservation = Guid.Empty };
        }
    }
}