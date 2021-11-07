using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Domain.Exceptions;
using DistributedWarehouses.Domain.Repositories;
using DistributedWarehouses.Dto;

namespace DistributedWarehouses.DomainServices
{
    public class DistributionService
    {
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IRepository _entityRepository;
        private DistributableItemEntity _distributableItem;
        private readonly string _sortBy;
        private List<(Guid, int)> _warehouseItems;
        private int _quantity;
        private Guid _warehouse;
        private int _availableQuantity;

        public DistributionService(DistributableItemEntity distributableItem, IWarehouseRepository warehouseRepository,
            IRepository entityRepository, string sortBy)
        {
            _sortBy = sortBy;
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

            if (_quantity > 0)
            {
                throw new InsufficientStorageException(_quantity);
            }

            return _warehouseItems;
        }

        private async Task<(Guid, int)> GetWarehouseParams()
        {
            var warehouseItem = await _warehouseRepository.GetWarehouseByItem(_distributableItem.Item, _sortBy);
            return (warehouseItem.Id, (int)typeof(WarehouseInformation).GetProperty(_sortBy).GetValue(warehouseItem));
        }

        private async Task Add(int quantityToAdd)
        {
            _distributableItem.Quantity = quantityToAdd;
            _distributableItem.Warehouse = _warehouse;
            await _entityRepository.Add(_distributableItem);
            if (_distributableItem is InvoiceItemEntity)
            {
                await _warehouseRepository.UpdateWarehouseItemQuantity(_distributableItem.Item, _warehouse,
                    quantityToAdd);
            }
        }
    }
}