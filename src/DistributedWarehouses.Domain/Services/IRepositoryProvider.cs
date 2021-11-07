using DistributedWarehouses.Domain.Repositories;

namespace DistributedWarehouses.Domain.Services
{
    public interface IRepositoryProvider
    {
        public T GetRepository<T>() where T : IRepository;
    }
}