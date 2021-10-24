using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DistributedWarehouses.Domain;
using DistributedWarehouses.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Item = DistributedWarehouses.Domain.Entities.Item;

namespace DistributedWarehouses.Infrastructure.Repositories
{
    public class ItemsRepository : IItemsRepository
    {
        private readonly DistributedWarehousesContext _distributedWarehousesContext;
        public ItemsRepository(DistributedWarehousesContext distributedWarehousesContext)
        {
            _distributedWarehousesContext = distributedWarehousesContext;
        }
        public IEnumerable<Item> GetItems()
        {
            return  _distributedWarehousesContext.Items.Select(i => new Item
            {
                SKU = i.Sku,
                Title = i.Title
            }).AsEnumerable();
        }

        public Task<int> AddItem(Item item)
        {
            _distributedWarehousesContext.Items.Add(new Models.Item
            {
                Sku = item.SKU,
                Title = item.Title
            });
            return _distributedWarehousesContext.SaveChangesAsync();
        }
    }
}
