using DistributedWarehouses.Domain.Repositories;
using DistributedWarehouses.Domain.Resources;
using FluentValidation;

namespace DistributedWarehouses.ApplicationServices.Validators
{
    /// <summary>
    /// <param type="bool">Is object new</param>
    /// /// <param type="string">Object sku</param>
    /// </summary>
    public class SkuValidator : AbstractValidator<(bool, string)>
    {
        private readonly string _skuPattern;
        private readonly IItemRepository _itemRepository;

        public SkuValidator(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
            _skuPattern = "^[a-zA-Z0-9]*$";
            InputRule();
            ExistenceRule();
        }

        private void InputRule()
        {
            RuleFor(sku => sku.Item2).Cascade(CascadeMode.Stop).NotNull().NotEmpty().Matches(_skuPattern)
                .WithMessage(string.Format(ErrorMessageResource.NotSupported, "sku", _skuPattern)).WithErrorCode("400");
        }

        private void ExistenceRule()
        {
            RuleFor(sku => sku.Item2).MustAsync(async (sku, cancellation) => await _itemRepository.ExistsAsync(sku))
                .When(sku => !sku.Item1)
                .WithMessage(string.Format(ErrorMessageResource.NotFound, "sku")).WithErrorCode("404");
            RuleFor(sku => sku.Item2).MustAsync(async (sku, cancellation) => !await _itemRepository.ExistsAsync(sku))
                .When(sku => sku.Item1);
        }
    }
}