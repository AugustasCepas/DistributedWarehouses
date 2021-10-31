using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Resources;
using FluentValidation;

namespace DistributedWarehouses.DomainServices.Validators
{
    public class SkuValidator : AbstractValidator<string>
    {
        private string _skuPattern;
        public SkuValidator()
        {
            _skuPattern = "^[[a-zA-Z0-9]]*$";
            RuleFor(sku => sku).NotNull().NotEmpty().Matches(_skuPattern)
                .WithMessage(string.Format(ErrorMessageResource.NotSupportedSku, "sku", _skuPattern));
        }
    }
}
