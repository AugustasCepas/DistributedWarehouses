using System.Collections.Generic;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;

namespace DistributedWarehouses.Domain
{
  public interface IWarehouseItemRepository
    {
        public IEnumerable<WarehouseItem> GetWarehouseItem(string sku);
        public Task<int> AddWarehouseItem(Item item);
        public Task<int> RemoveWarehouseItem(string sku);
    }
}
