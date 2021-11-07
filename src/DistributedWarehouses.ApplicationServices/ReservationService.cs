using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Domain.Exceptions;
using DistributedWarehouses.Domain.Repositories;
using DistributedWarehouses.Domain.Resources;
using DistributedWarehouses.Domain.Services;
using DistributedWarehouses.Domain.Validators;
using DistributedWarehouses.DomainServices;
using DistributedWarehouses.Dto;
using FluentValidation;

namespace DistributedWarehouses.ApplicationServices
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IMappingService _mappingService;
        private readonly IValidator<string, IItemRepository> _skuValidator;
        private readonly IValidator<Guid, IReservationRepository> _reservationGuidValidator;

        public ReservationService(IReservationRepository reservationRepository,
            IWarehouseRepository warehouseRepository, IMappingService mappingService, IValidator<string, IItemRepository> skuValidator, IValidator<Guid, IReservationRepository> reservationGuidValidator)
        {
            _reservationRepository = reservationRepository;
            _warehouseRepository = warehouseRepository;
            _mappingService = mappingService;
            _skuValidator = skuValidator;
            _reservationGuidValidator = reservationGuidValidator;
        }

        public ReservationEntity GetReservation(Guid id)
        {
            var result = _reservationRepository.GetReservation(id);

            return result;
        }

        public async Task<IdDto> AddReservationAsync(ReservationInputDto reservationInputDto)
        {
            if (reservationInputDto.Reservation is not null)
            {
                await _reservationGuidValidator.ValidateAsync((Guid) reservationInputDto.Reservation, false);
            }
            await _skuValidator.ValidateAsync(reservationInputDto.ItemSku, false);
            if (reservationInputDto.Quantity <= 0)
            {
                throw new BadRequestException(string.Format(ErrorMessageResource.NotSupported, "quantity", "Larger than 0"));

            }
            reservationInputDto.Reservation ??= Guid.NewGuid();
            var reservationItem = _mappingService.Map<ReservationItemEntity>(reservationInputDto);
            return new IdDto(await ReserveItemInWarehouseAsync(reservationItem));
        }

        public async Task RemoveReservationAsync(Guid id)
        {
            await _reservationGuidValidator.ValidateAsync(id, false);
            await _reservationRepository.RemoveReservationAsync(id);
        }


        public async Task <IEnumerable<ReservationItemEntity>> GetReservationItemsByReservation(Guid reservationId)
        {
            await _reservationGuidValidator.ValidateAsync(reservationId, false);
            return _reservationRepository.GetReservationItems(reservationId);
        }

        public async Task<ReservationRemovedDto> RemoveReservationItemAsync(string item, Guid reservation)
        {
            await _skuValidator.ValidateAsync(item, false);
            await _reservationGuidValidator.ValidateAsync(reservation, false);
            var reservationItems = _reservationRepository.GetReservationItems(reservation, item);
            var result =
                new ReservationRemovedDto(await _reservationRepository.RemoveReservationItem(reservationItems));

            if (!_reservationRepository.GetReservationItems(reservation).Any())
                await _reservationRepository.RemoveReservationAsync(reservation);

            return result;
        }

        private async Task<Guid> ReserveItemInWarehouseAsync(ReservationItemEntity reservationItem)
        {
            using (var transaction = _reservationRepository.GetTransaction())
            {
                try
                {
                    await AddReservationAsync(reservationItem.Reservation);
                    await AddReservationItemsToWarehousesAsync(reservationItem);
                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }

            return reservationItem.Reservation;
        }

        private async Task AddReservationItemsToWarehousesAsync(ReservationItemEntity reservation)
        {
            await new DistributionService(reservation, _warehouseRepository, _reservationRepository,
                nameof(WarehouseInformation.AvailableItemQuantity)).Distribute();
        }

        private Task<ReservationEntity> AddReservationAsync(Guid reservationId)
        {
            return _reservationRepository.AddReservationAsync(new ReservationEntity(reservationId));
        }
    }
}