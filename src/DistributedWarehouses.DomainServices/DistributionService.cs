using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Domain.Exceptions;
using DistributedWarehouses.Domain.Repositories;

namespace DistributedWarehouses.DomainServices
{
    public class DistributionService
    {
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IRepository _entityRepository;
        private readonly DistributableItemEntity _distributableItem;
        private List<(Guid, int)> _warehouseItems;
        private int _quantity;
        private Guid _warehouse;
        private int _availableQuantity;

        public DistributionService(DistributableItemEntity distributableItem, IWarehouseRepository warehouseRepository, IRepository entityRepository)
        {
            _distributableItem = distributableItem;
            _warehouseItems = new List<(Guid, int)>();
            _quantity = distributableItem.Quantity;
            _warehouseRepository = warehouseRepository;
            _entityRepository = entityRepository;
        }

        public async Task<IEnumerable<(Guid, int)>> Distribute()
        {
            (_warehouse, _availableQuantity) = await GetWarehouseParams();
            while (_quantity > 0 && _availableQuantity > 0)
            {
                var toAdd = _availableQuantity >= _quantity ? _quantity : _availableQuantity;
                await Add(toAdd);
                _warehouseItems.Add((_warehouse, toAdd));
                _quantity -= toAdd;
                (_warehouse, _availableQuantity) = await GetWarehouseParams();
            }

            if (_quantity>0)
            {
                throw new InsufficientStorageException(_quantity);
            }
            return _warehouseItems;
        }

        private async Task<(Guid, int)> GetWarehouseParams()
        {
            var warehouseItem =
                await _warehouseRepository.GetLargestWarehouseByFreeItemsQuantityAsync(_distributableItem.Item);
            return (warehouseItem.Warehouse, warehouseItem.Quantity);
        }

        private async Task Add(int quantityToAdd)
        {
            _distributableItem.Quantity = quantityToAdd;
            _distributableItem.Warehouse = _warehouse;
            await _entityRepository.Add<DistributableItemEntity>(_distributableItem);
        }
    }
}