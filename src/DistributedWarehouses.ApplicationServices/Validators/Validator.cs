using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Exceptions;
using FluentValidation;

namespace DistributedWarehouses.ApplicationServices.Validators
{
    public class Validator<T, TRepository> : Domain.Validators.IValidator<T, TRepository>
    {
        private readonly IValidator<T> _validator;

        public Validator(IValidator<T> validator)
        {
            _validator = validator;
        }
        public async Task ValidateAsync(T param)
        {
            var validationResult = await _validator.ValidateAsync(param);
            if (!validationResult.IsValid)
            {
                throw new BaseException(validationResult.Errors.First().ErrorMessage, int.Parse(validationResult.Errors.First().ErrorCode));
            }
        }
    }
}
