using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Repositories;
using DistributedWarehouses.Domain.Resources;
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
            RuleFor(id => id).Cascade(CascadeMode.Stop).NotEmpty().NotNull();
        }


    }
}
