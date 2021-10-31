using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Repositories;
using DistributedWarehouses.Domain.Resources;
using FluentValidation;
using FluentValidation.Results;

namespace DistributedWarehouses.DomainServices.Validators
{
    public class SkuValidator : AbstractValidator<string>
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
            RuleFor(sku => sku).NotNull().NotEmpty().Matches(_skuPattern)
                .WithMessage(string.Format(ErrorMessageResource.NotSupported, "sku", _skuPattern)).WithErrorCode("400");
        }

        private void ExistenceRule()
        {
            RuleFor(sku => sku).MustAsync(async (sku, cancellation) => await _itemRepository.ExistsAsync(sku))
                .WithMessage(string.Format(ErrorMessageResource.NotFound, "sku")).WithErrorCode("404");
        }
    }
}
