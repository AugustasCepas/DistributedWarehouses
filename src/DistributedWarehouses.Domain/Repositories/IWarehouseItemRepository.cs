using System.Collections.Generic;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;

namespace DistributedWarehouses.Domain
{
  public interface IWarehouseItemRepository
    {
        public IEnumerable<WarehouseItemEntity> GetWarehouseItem(string sku);
        public Task<int> AddWarehouseItem(ItemEntity item);
        public Task<int> RemoveWarehouseItem(string sku);
    }
}
