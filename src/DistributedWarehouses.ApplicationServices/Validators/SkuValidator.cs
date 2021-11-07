using DistributedWarehouses.Domain.Resources;
using FluentValidation;

namespace DistributedWarehouses.ApplicationServices.Validators
{
    /// <summary>
    /// <param type="bool">Is object new</param>
    /// /// <param type="string">Object sku</param>
    /// </summary>
    public class SkuValidator : AbstractValidator<string>
    {
        private readonly string _skuPattern;

        public SkuValidator()
        {
            _skuPattern = "^[a-zA-Z0-9]*$";
            InputRule();
        }

        private void InputRule()
        {
            RuleFor(sku => sku).Cascade(CascadeMode.Stop).NotNull().NotEmpty().Matches(_skuPattern)
                .WithMessage(string.Format(ErrorMessageResource.NotSupported, "sku", _skuPattern)).WithErrorCode("400");
        }
    }
}