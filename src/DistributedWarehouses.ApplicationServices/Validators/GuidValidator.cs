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
    public class GuidValidator<T> : AbstractValidator<(bool,Guid)> where T : IRepository
    {
        private readonly T  _repository;

        public GuidValidator(T repository)
        {
            _repository = repository;
            InputRule();
            ExistenceRule();
        }

        private void InputRule()
        {
            RuleFor(id => id.Item2).Cascade(CascadeMode.Stop).NotEmpty().NotNull();
        }

        private void ExistenceRule()
        {
            RuleFor(id => id.Item2).MustAsync(async (id, cancellation) => await _repository.ExistsAsync(id)).When(id => !id.Item1)
                .WithMessage(string.Format(ErrorMessageResource.NotFound, "id")).WithErrorCode("404");

            RuleFor(id => id.Item2).MustAsync(async (id, cancellation) => !await _repository.ExistsAsync(id))
                .When(id => id.Item1);
        }
    }
}
