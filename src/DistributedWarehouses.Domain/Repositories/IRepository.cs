using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;

namespace DistributedWarehouses.Domain.Repositories
{
    public interface IRepository
    {
        public Task<bool> ExistsAsync<T>(T id);
        public Task Add<T>(T entity) where T : DistributableItemEntity;
    }
}
