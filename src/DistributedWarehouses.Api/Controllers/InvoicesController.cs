using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DistributedWarehouses.Api.Swagger;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Domain.Services;
using DistributedWarehouses.Dto;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Filters;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DistributedWarehouses.Api.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public InvoicesController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        /// <summary>
        /// 9) Return list of all Invoices
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<InvoiceEntity>), StatusCodes.Status200OK)]
        public IActionResult ReturnListOfAllInvoices()
        {
            var response = _invoiceService.GetInvoices();

            return Ok(response);
        }

        /// <summary>
        /// 10) Return info about one Invoice
        /// </summary>
        [HttpGet("{id:guid}", Name = "GetInvoiceById")]
        [ProducesResponseType(typeof(InvoiceDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> ReturnInfoAboutOneInvoice(Guid id)
        {
            var item = await _invoiceService.GetInvoiceItemsAsync(id);

            return Ok(item);
        }

        /// <summary>
        /// 5) SKU is sold
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("sell-item")]
        [ProducesResponseType(typeof(IdDto), StatusCodes.Status200OK)]
        [SwaggerRequestExample(typeof(ItemSellDto), typeof(ItemSellDtoExample))]
        public async Task<IActionResult> SellWarehouseItem(ItemSellDto dto)
        {
            var result = await _invoiceService.SellItems(dto);
            var link = Url.Link("GetInvoiceById", new { id = result.Id });
            return Created(link, result);
        }

        /// <summary>
        /// 11) Return all goods within invoice
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("{id:guid}/return")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        public async Task<IActionResult> ReturnGoodsFromInvoice(Guid id)
        {
            var response = await _invoiceService.ReturnGoodsFromInvoice(id);
            return Ok(response);
        }
    }
}