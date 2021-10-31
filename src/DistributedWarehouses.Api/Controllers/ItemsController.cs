using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Domain.Resources;
using DistributedWarehouses.Domain.Services;
using DistributedWarehouses.Dto;
using DistributedWarehouses.Infrastructure.Models;
using FluentValidation;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DistributedWarehouses.Api.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IItemService _itemService;
        private readonly IValidator<string> _validator;

        public ItemsController(IItemService itemService, IValidator<string> validator)
        {
            _itemService = itemService;
            _validator = validator;
        }

        // Return list of all SKUs
        // GET: <ItemsController>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ItemEntity>), StatusCodes.Status200OK)]
        public IActionResult ReturnListOfAllSKUs()
        {
            var response = _itemService.GetItems();

            return Ok(response);
        }

        // Return info about one SKU
        // How many items left in each warehouseEntity
        // How many items are reserved
        // TODO: How many items are planned to be delivered soon
        // GET: <ItemsController>/$SKU
        [HttpGet("{sku}")]
        [ProducesResponseType(typeof(ItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ReturnInfoAboutOneSKU (string sku)
        {
            var item = await _itemService.GetItemInWarehousesInfo(sku);
            return Ok(item);
        }

        // // POST <ItemsController>/items
        // [HttpPost]
        // [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        // public async Task<IActionResult> Post([FromBody] ItemEntity item)
        // {
        //     var result = await _itemService.AddItem(item);
        //     return Ok(result);
        // }
        //
        // // DELETE <ItemsController>/items/$SKU
        // [HttpDelete("{SKU}")]
        // [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        // public async Task<IActionResult> Delete(string SKU)
        // {
        //     var result = await _itemService.RemoveItem(SKU);
        //     return Ok(result);
        // }
    }
}