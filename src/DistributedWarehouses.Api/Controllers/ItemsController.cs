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

        /// <summary>
        /// 1) Return list of all SKUs
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ItemEntity>), StatusCodes.Status200OK)]
        public IActionResult ReturnListOfAllSKUs()
        {
            var response = _itemService.GetItems();
            return Ok(response);
        }

        /// <summary>
        /// 2) Return info about one SKU
        /// </summary>
        [HttpGet("{sku}")]
        [ProducesResponseType(typeof(ItemDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> ReturnInfoAboutOneSKU(string sku)
        {
            var item = await _itemService.GetItemInWarehousesInfoAsync(sku);
            return Ok(item);
        }
    }
}