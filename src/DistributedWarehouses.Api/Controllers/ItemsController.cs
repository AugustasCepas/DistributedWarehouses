using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Domain.Services;
using DistributedWarehouses.Dto;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace DistributedWarehouses.Api.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemsController(IItemService itemService, IValidator<string> validator)
        {
            _itemService = itemService;
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
        public async Task<IActionResult> ReturnInfoAboutOneSKU(string sku)
        {
            var item = await _itemService.GetItemInWarehousesInfoAsync(sku);
            return Ok(item);
        }
    }
}