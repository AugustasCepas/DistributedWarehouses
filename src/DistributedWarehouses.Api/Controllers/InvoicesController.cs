using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Domain.Services;
using DistributedWarehouses.Dto;
using Microsoft.AspNetCore.Http;

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

        // Return list of all Invoices
        // GET: <InvoicesController>/Invoices
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<InvoiceEntity>), StatusCodes.Status200OK)]
        public IActionResult ReturnListOfAllInvoices()
        {
            var response = _invoiceService.GetInvoices();

            return Ok(response);
        }

        // Return info about one Invoice
        // GET: <InvoicesController>/$invoiceGuid
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(InvoiceDto), StatusCodes.Status200OK)]
        public IActionResult ReturnInfoAboutOneInvoice(Guid id)
        {
            var item = _invoiceService.GetInvoiceItems(id);

            return Ok(item);
        }

        // // GET api/<InvoicesController>/5
        // [HttpGet("{id}")]
        // [ProducesResponseType(typeof(WarehouseEntity), StatusCodes.Status200OK)]
        // public IActionResult Get(Guid id)
        // {
        //     var result = _invoiceService.GetInvoice(id);
        //     return Ok(result);
        // }

        // // POST api/<InvoicesController>
        // [HttpPost]
        // [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        // public async Task<IActionResult> Post([FromBody] InvoiceEntity invoiceEntity)
        // {
        //     var result = await _invoiceService.AddInvoice(invoiceEntity);
        //     return Ok(result);
        // }

        // // DELETE api/<InvoicesController>/5
        // [HttpDelete("{id}")]
        // [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        // public async Task<IActionResult> Delete(Guid id)
        // {
        //     var result = await _invoiceService.RemoveInvoice(id);
        //     return Ok(result);
        // }


        // Invoice Items

        // // Return list of all InvoiceItems
        // // GET: <InvoiceItemsController>/InvoiceItems
        // [HttpGet]
        // [ProducesResponseType(typeof(IEnumerable<InvoiceItemEntity>), StatusCodes.Status200OK)]
        // public IActionResult Get()
        // {
        //     var response = _invoiceService.GetInvoiceItems();
        //
        //     return Ok(response);
        // }

        // // GET api/<InvoiceItemsController>/5
        // [HttpGet("{id}")]
        // [ProducesResponseType(typeof(WarehouseEntity), StatusCodes.Status200OK)]
        // public IActionResult Get(Guid id)
        // {
        //     var result = _invoiceItemService.GetInvoiceItem(id);
        //     return Ok(result);
        // }

        // // POST api/<InvoiceItemsController>
        // [HttpPost]
        // [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        // public async Task<IActionResult> Post([FromBody] InvoiceItemEntity invoiceItemEntity)
        // {
        //     var result = await _invoiceService.AddInvoiceItem(invoiceItemEntity);
        //     return Ok(result);
        // }

        // // DELETE api/<InvoiceItemsController>/5
        // [HttpDelete("{id}")]
        // [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        // public async Task<IActionResult> Delete(Guid id)
        // {
        //     var result = await _invoiceItemService.RemoveInvoiceItem(id);
        //     return Ok(result);
        // }
    }
}