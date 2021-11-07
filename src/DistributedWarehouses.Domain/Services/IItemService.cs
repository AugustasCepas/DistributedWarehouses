using System.Collections.Generic;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Dto;

namespace DistributedWarehouses.Domain.Services
{
    public interface IItemService
    {
        IEnumerable<ItemEntity> GetItems();
        Task<ItemDto> GetItemInWarehousesInfoAsync(string sku);
    }
}