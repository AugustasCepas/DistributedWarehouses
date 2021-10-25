using System.Collections.Generic;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Dto;

namespace DistributedWarehouses.Domain
{
    public interface IItemsRepository
    {
        public IEnumerable<Item> GetItems();
        public Item GetItem(string SKU);
        public Task<int> AddItem(Item item);
        public Task<int> RemoveItem(string SKU);
        public IEnumerable<ItemInWarehousesInfoDto> GetItemsInWarehouses(string sku);

    }
}
