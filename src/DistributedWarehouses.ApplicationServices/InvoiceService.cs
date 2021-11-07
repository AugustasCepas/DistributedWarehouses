using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Domain.Repositories;
using DistributedWarehouses.Domain.Services;
using DistributedWarehouses.Domain.Validators;
using DistributedWarehouses.DomainServices;
using DistributedWarehouses.Dto;

namespace DistributedWarehouses.ApplicationServices
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IMappingService _mappingService;
        private readonly IValidator<Guid, IInvoiceRepository> _invoiceGuidValidator;
        private readonly IValidator<string, IItemRepository> _skuValidator;
        private readonly IValidator<Guid, IReservationRepository> _reservationGuidValidator;
        private readonly IValidator<ItemSellDto, IInvoiceRepository> _itemSellValidator;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IReservationService _reservationService;

        public InvoiceService(IMappingService mappingService,
            IValidator<Guid, IInvoiceRepository> invoiceGuidValidator,
            IValidator<string, IItemRepository> skuValidator,
            IValidator<Guid, IReservationRepository> reservationGuidValidator,
            IInvoiceRepository invoiceRepository,
            IWarehouseRepository warehouseRepository, IReservationService reservationService,
            IValidator<ItemSellDto, IInvoiceRepository> itemSellValidator)
        {
            _mappingService = mappingService;
            _invoiceGuidValidator = invoiceGuidValidator;
            _skuValidator = skuValidator;
            _reservationGuidValidator = reservationGuidValidator;
            _invoiceRepository = invoiceRepository;
            _warehouseRepository = warehouseRepository;
            _reservationService = reservationService;
            _itemSellValidator = itemSellValidator;
        }

        public IEnumerable<InvoiceEntity> GetInvoices()
        {
            return _invoiceRepository.GetInvoices();
        }

        public async Task<InvoiceDto> GetInvoiceItemsAsync(Guid id)
        {
            await _invoiceGuidValidator.ValidateAsync(id, false);
            var invoice = _mappingService.Map<InvoiceDto>(await GetInvoiceAsync(id));
            var invoiceItems = _invoiceRepository.GetInvoiceItems(id);
            invoice.Items = _mappingService.Map<IEnumerable<ItemInInvoiceInfoDto>>(invoiceItems);
            return invoice;
        }


        public async Task<IdDto> SellItems(ItemSellDto dto)
        {
            await _itemSellValidator.ValidateAsync(dto, true);
            if (dto.SKU is not null)
            {
                await _skuValidator.ValidateAsync(dto.SKU, false);
            }
            if (dto.InvoiceId is not null)
            {
                await _invoiceGuidValidator.ValidateAsync((Guid)dto.InvoiceId, false);
            }

            if (dto.ReservationId is not null)
            {
                await _reservationGuidValidator.ValidateAsync((Guid) dto.ReservationId, false);
            }

            using (var transaction = _invoiceRepository.GetTransaction())
            {
                try
                {
                    dto.InvoiceId = dto.InvoiceId ??= Guid.NewGuid();
                    await AddInvoice((Guid) dto.InvoiceId);
                    await AddInvoiceItemsAsync(dto);
                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }

            return new IdDto((Guid) dto.InvoiceId);
        }

        public async Task<int> ReturnGoodsFromInvoice(Guid id)
        {
            await _invoiceGuidValidator.ValidateAsync(id, false);
            return await ReturnInvoice(id);
        }

        private async Task<InvoiceEntity> GetInvoiceAsync(Guid id)
        {
            await _invoiceGuidValidator.ValidateAsync(id, false);
            return await _invoiceRepository.GetInvoiceAsync(id);
        }

        private async Task AddInvoiceItemsAsync(ItemSellDto dto)
        {
            var items = new List<InvoiceItemEntity>();
            if (dto.SKU is not null)
            {
                items.Add(new InvoiceItemEntity
                    {Invoice = (Guid) dto.InvoiceId, Item = dto.SKU, Quantity = (int) dto.Quantity});
            }
            else
            {
                items = _mappingService.Map<List<InvoiceItemEntity>>(
                    _reservationService.GetReservationItemsByReservation((Guid) dto.ReservationId));
                await _reservationService.RemoveReservationAsync((Guid) dto.ReservationId);
            }

            foreach (var item in items)
            {
                item.Invoice = (Guid) dto.InvoiceId;
                await new DistributionService(item, _warehouseRepository, _invoiceRepository,
                    nameof(WarehouseInformation.AvailableItemQuantity)).Distribute();
            }
        }

        private async Task<InvoiceEntity> AddInvoice(Guid id)
        {
            var invoice = new InvoiceEntity
            {
                Id = id
            };
            return await _invoiceRepository.AddInvoiceAsync(invoice);
        }

        /// <summary>
        /// Return Goods From Invoice
        /// Get Invoice Items
        /// Get Warehouse To Return Items
        /// Return Items To Warehouse
        /// Remove Invoice Items
        /// Remove Invoice
        /// </summary>
        /// <param name="id">Warehouse guid</param>
        /// <returns></returns>
        private async Task<int> ReturnInvoice(Guid id)
        {
            using (var transaction = _invoiceRepository.GetTransaction())
            {
                try
                {
                    var invoiceItems = _invoiceRepository.GetInvoiceItems(id).ToList();
                    await ReturnItemsToWarehouses(invoiceItems);
                    await RevertInvoice(id);
                    await transaction.CommitAsync();
                    return invoiceItems.Count;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        private async Task ReturnItemsToWarehouses(List<InvoiceItemEntity> invoiceItems)
        {
            foreach (var item in invoiceItems)
            {
                await new DistributionService(_mappingService.Map<WarehouseItemEntity>(item), _warehouseRepository,
                    _warehouseRepository, nameof(WarehouseInformation.FreeQuantity)).Distribute();
            }
        }

        private Task<int> RevertInvoice(Guid id)
        {
            return _invoiceRepository.RevertInvoice(id);
        }
    }
}