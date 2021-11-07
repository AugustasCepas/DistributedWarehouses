using System.Threading.Tasks;
using DistributedWarehouses.Domain.Repositories;

namespace DistributedWarehouses.Domain.Validators
{
    public interface IValidator<T, TRepository> where TRepository : IRepository
    {
        public Task ValidateAsync(T param, bool isNew);
    }
}
