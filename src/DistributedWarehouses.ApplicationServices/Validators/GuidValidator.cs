using System;
using FluentValidation;

namespace DistributedWarehouses.ApplicationServices.Validators
{
    /// <summary>
    /// <param type="bool">Is object new</param>
    /// /// <param type="Guid">Object id</param>
    /// </summary>
    public class GuidValidator : AbstractValidator<Guid>
    {
        public GuidValidator()
        {
            InputRule();
        }

        private void InputRule()
        {
            RuleFor(id => id).Cascade(CascadeMode.Stop).NotEmpty().WithErrorCode("400").NotNull().WithErrorCode("400").WithName("Id");
        }
    }
}