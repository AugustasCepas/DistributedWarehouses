﻿using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using DistributedWarehouses.ApplicationServices;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Dto;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DistributedWarehouses.Api.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemController(
            IItemService itemService)
        {
            _itemService = itemService;
        }

        // Task End Points

        // Return list of all SKUs
        // GET: <ItemController>/items
        [HttpGet("items")]
        [ProducesResponseType(typeof(IEnumerable<ItemEntity>), StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            var response = _itemService.GetItems();

            return Ok(response);
        }

        // // Return info about one SKU
        // // How many items left in each warehouseEntity
        // // How many items are reserved
        // // TODO: How many items are planned to be delivered soon
        // // GET: <ItemController>/items/$SKU
        [HttpGet("items/{SKU}")]
        [ProducesResponseType(typeof(ItemDto), StatusCodes.Status200OK)]
        public IActionResult Get(string SKU)
        {
            var item = _itemService.GetItem(SKU);

            return Ok(item);
        }

        // Other End Points
        // POST <ItemController>/items
        [HttpPost("items")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public async Task<IActionResult> Post([FromBody] ItemEntity item)
        {
            var result = await _itemService.AddItem(item);
            return Ok(result);
        }

        // DELETE <ItemController>/items/$SKU
        [HttpDelete("items/{SKU}")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(string SKU)
        {
            var result = await _itemService.RemoveItem(SKU);
            return Ok(result);
        }
    }
}