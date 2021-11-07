using AutoMapper;
using System;
using System.Collections.Generic;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Dto;
using DistributedWarehouses.Infrastructure.Models;

namespace DistributedWarehouses.Infrastructure.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Invoice, InvoiceEntity>().ReverseMap();
            CreateMap<Reservation, ReservationEntity>().ReverseMap();
            CreateMap<Warehouse, WarehouseEntity>().ReverseMap();
            CreateMap<Item, ItemEntity>()
                .ForMember(destination => destination.SKU, opts => opts.MapFrom(source => source.Sku));

            CreateMap<InvoiceItem, InvoiceItemEntity>().ReverseMap();
            CreateMap<WarehouseItem, WarehouseItemEntity>().ReverseMap();
            CreateMap<ReservationItem,ReservationItemEntity>().ReverseMap();

            CreateMap<InvoiceItemEntity, ReservationItemEntity>().ReverseMap();
            CreateMap<InvoiceItemEntity, WarehouseItemEntity>();

            CreateMap<WarehouseInformation, WarehouseEntity>();
            CreateMap<InvoiceItemEntity, WarehouseItem>();

            CreateMap<Invoice, InvoiceDto>();
            CreateMap<InvoiceEntity, InvoiceDto>();

            CreateMap<InvoiceItem, ItemInInvoiceInfoDto>()
                .ForMember(destination => destination.ItemId, opts => opts.MapFrom(source => source.Item))
                .ForMember(destination => destination.PurchasedQuantity,
                    opts => opts.MapFrom(source => source.Quantity))
                .ForMember(destination => destination.WarehouseId, opts => opts.MapFrom(source => source.Warehouse));
            CreateMap<ItemEntity, ItemDto>();
            CreateMap<ItemSellDto, InvoiceItemEntity>()
                .ForMember(destination => destination.Item, opts => opts.MapFrom(source => source.SKU));
            CreateMap<ReservationEntity, IdDto>()
                .ForMember(destination => destination.Id, opts => opts.MapFrom(source => source.Id));
            CreateMap<InvoiceItemEntity, ItemInInvoiceInfoDto>()
                .ForMember(destination => destination.WarehouseId, opts => opts.MapFrom(source => source.Warehouse))
                .ForMember(destination => destination.ItemId, opts => opts.MapFrom(source => source.Item))
                .ForMember(destination => destination.PurchasedQuantity,
                    opts => opts.MapFrom(source => source.Quantity));
            CreateMap<Tuple<Guid, int, int>, ItemInWarehousesInfoDto>()
                .ForMember(destination => destination.WarehouseId, opts => opts.MapFrom(source => source.Item1))
                .ForMember(destination => destination.StoredQuantity, opts => opts.MapFrom(source => source.Item2))
                .ForMember(destination => destination.ReservedQuantity, opts => opts.MapFrom(source => source.Item3));
            CreateMap<Tuple<string, int, Guid, Guid>, InvoiceItemEntity>()
                .ForMember(destination => destination.Item, opts => opts.MapFrom(source => source.Item1))
                .ForMember(destination => destination.Quantity, opts => opts.MapFrom(source => source.Item2))
                .ForMember(destination => destination.Warehouse, opts => opts.MapFrom(source => source.Item3));
            CreateMap<ReservationInputDto, ReservationItemEntity>()
                .ForMember(destination => destination.Item, opts => opts.MapFrom(source => source.ItemSku));
            CreateMap<WarehouseEntity, WarehouseDto>()
                .ForMember(destination => destination.FreeQuantity, opts => opts.MapFrom(source => source.Capacity));
        }
    }
}