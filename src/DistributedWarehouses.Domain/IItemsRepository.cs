using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;

namespace DistributedWarehouses.Domain
{
    public interface IItemsRepository
    {
        public IEnumerable<Item> GetItems();
        public Task<int> AddItem(Item item);
    }
}
