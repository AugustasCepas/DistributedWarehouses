using DistributedWarehouses.Domain.Entities;
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
