using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Dto;

namespace DistributedWarehouses.Domain.Services
{
    public interface IInvoiceService
    {
        IEnumerable<InvoiceEntity> GetInvoices();
        Task<InvoiceDto> GetInvoiceItemsAsync(Guid id);
        public Task<IdDto> SellItems(ItemSellDto dto);
        Task<int> ReturnGoodsFromInvoice(Guid id);
    }
}