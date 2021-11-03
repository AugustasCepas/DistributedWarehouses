using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Dto;
using DistributedWarehouses.Infrastructure.Models;

namespace DistributedWarehouses.Infrastructure.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Invoice, InvoiceEntity>();
            CreateMap<InvoiceItem, InvoiceItemEntity>();
            CreateMap<Item, ItemEntity>()
                .ForMember(destination => destination.SKU, opts => opts.MapFrom(source => source.Sku));
            CreateMap<Reservation, ReservationEntity>();
            CreateMap<ReservationItem, ReservationItemEntity>();
            CreateMap<Warehouse, WarehouseEntity>();
            CreateMap<WarehouseItem, WarehouseItemEntity>();
            CreateMap<Invoice, InvoiceDto>();
            CreateMap<InvoiceItem, ItemInInvoiceInfoDto>()
                .ForMember(destination => destination.ItemId, opts => opts.MapFrom(source => source.Item))
                .ForMember(destination => destination.PurchasedQuantity, opts => opts.MapFrom(source => source.Quantity))
                .ForMember(destination => destination.WarehouseId, opts => opts.MapFrom(source => source.Warehouse));
            CreateMap<ItemEntity, ItemDto>();
            CreateMap<ItemSellDto, InvoiceItemEntity>()
                .ForMember(destination => destination.Item, opts => opts.MapFrom(source => source.SKU))
                .ForMember(destination => destination.Warehouse, opts => opts.MapFrom(source => source.WarehouseId));
            CreateMap<ReservationEntity, ReservationIdDto>()
                .ForMember(destination => destination.ReservationId, opts => opts.MapFrom(source => source.Id));

        }
    }
}
