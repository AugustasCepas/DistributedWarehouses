using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        public async Task<IActionResult> ReturnInfoAboutOneInvoice(Guid id)
        {
            var item = await _invoiceService.GetInvoiceItemsAsync(id);

            return Ok(item);
        }

        // Return all goods within invoice
        // POST: invoices/{id}
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public async Task<IActionResult> ReturnGoodsFromInvoice(Guid id)
        {
            var response = await _invoiceService.ReturnGoodsFromInvoice(id);
            return Ok(response);
        }
    }
}