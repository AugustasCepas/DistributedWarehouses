using System.Linq;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Exceptions;
using DistributedWarehouses.Domain.Repositories;
using DistributedWarehouses.Domain.Resources;
using DistributedWarehouses.Domain.Services;
using FluentValidation;

namespace DistributedWarehouses.ApplicationServices.Validators
{
    public class Validator<T, TRepository> : Domain.Validators.IValidator<T, TRepository> where TRepository:IRepository
    {
        private readonly IValidator<T> _validator;
        private readonly IRepositoryProvider _repositoryProvider;

        public Validator(IValidator<T> validator, IRepositoryProvider repositoryProvider)
        {
            _validator = validator;
            _repositoryProvider = repositoryProvider;
        }
        public async Task ValidateAsync(T param, bool isNew)
        {
            var validationResult = await _validator.ValidateAsync(param);
            if (!validationResult.IsValid)
            {
                int errorCode;
                int.TryParse(validationResult.Errors.First().ErrorCode, out errorCode);
                throw new BaseException(validationResult.Errors.First().ErrorMessage, errorCode);
            }
            await ValidateExistenceAsync(param, isNew);
        }

        private async Task ValidateExistenceAsync(T param, bool isNew)
        {
            if (isNew && await _repositoryProvider.GetRepository<TRepository>().ExistsAsync(param))
            {
                throw new BadRequestException(string.Format(ErrorMessageResource.NotSupported, "id", "Unique"));
            }
        
            if (!isNew && !await _repositoryProvider.GetRepository<TRepository>().ExistsAsync(param))
            {
                throw new NotFoundException("id");
            }
        }
    }
}
