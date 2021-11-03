using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;
using FluentValidation;

namespace DistributedWarehouses.ApplicationServices.Validators
{
    public class InvoiceValidator : AbstractValidator<InvoiceEntity>
    {
        public InvoiceValidator()
        {
            var inv = new InvoiceEntity
            {
                CreatedAt = DateTime.Now,
                Id = Guid.Empty
            };

            RuleFor(ie => ie.CreatedAt).NotEmpty().NotNull();
        }
    }
}
