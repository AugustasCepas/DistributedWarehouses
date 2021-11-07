using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Domain.Services;
using DistributedWarehouses.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DistributedWarehouses.Api.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class WarehousesController : ControllerBase
    {
        private readonly IWarehouseService _warehouseService;

        public WarehousesController(IWarehouseService warehouseService)
        {
            _warehouseService = warehouseService;
        }

        /// How many goods are stored
        /// How many goods are reserved
        /// How much free space available
        /// <summary>
        /// 7) Return info about one warehouse
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(WarehouseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> ReturnInfoOfOneWarehouse(Guid id)
        {
            var result = await _warehouseService.GetWarehouseInfo(id);
            return Ok(result);
        }

        /// <summary>
        /// 6) Return list of all Warehouses
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<WarehouseEntity>), StatusCodes.Status200OK)]
        public IActionResult ReturnListOfAllWarehouses()
        {
            var response = _warehouseService.GetWarehouses();

            return Ok(response);
        }

        // POST warehouses/
        /// <summary>
        /// 8) Add goods to warehouse
        /// </summary>
        /// <param name="warehouseItemEntity"></param>
        /// <returns></returns>
        [HttpPost("{warehouseId}/")]
        [ProducesResponseType(typeof(WarehouseItemEntity), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddWarehouseItem(Guid warehouseId, [BindRequired] string sku, [BindRequired] int quantity)
        {
            var result = await _warehouseService.AddWarehouseItem(warehouseId, sku, quantity);

            return Ok(result);
        }
    }
}