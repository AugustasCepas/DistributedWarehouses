using System;
using System.Net;
using DistributedWarehouses.Dto;
using FluentValidation;

namespace DistributedWarehouses.ApplicationServices.Validators
{
    public class ItemSellValidator : AbstractValidator<ItemSellDto>
    {
        public ItemSellValidator()
        {
            CascadeMode = CascadeMode.Stop;
            RuleFor(i => i.SKU).NotNull().When(i => i.ReservationId is null);
            RuleFor(i => i.Quantity).NotNull().When(i => i.SKU is not null);
            RuleFor(i => i.ReservationId).NotNull().NotEmpty().When(i => i.SKU is null);
            RuleFor(i => i.InvoiceId).NotEmpty().When(i => i.InvoiceId is not null);
            RuleFor(i => i.SKU).NotNull().When(i => !i.InvoiceId.Equals(Guid.Empty) && i.InvoiceId is not null);
        }
    }
}
