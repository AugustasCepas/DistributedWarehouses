using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Domain.Resources;
using FluentValidation;

namespace DistributedWarehouses.ApplicationServices.Validators
{
    public class ItemValidator : AbstractValidator<ItemEntity>
    {
        public ItemValidator()
        {
            RuleFor(i => i.Title).NotNull().NotEmpty();
        }
    }
}
