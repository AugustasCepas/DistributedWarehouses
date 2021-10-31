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
    public class WarehousesController : ControllerBase
    {
        private readonly IWarehouseService _warehouseService;

        public WarehousesController(IWarehouseService warehouseService)
        {
            _warehouseService = warehouseService;
        }

        //Return info about one warehouse
        //How many goods are stored
        //How many goods are reserved
        //TODO: How much free space available
        // GET <WarehousesController>/{id}
        [HttpGet("{warehouseGuid:guid}")]
        [ProducesResponseType(typeof(WarehouseDto), StatusCodes.Status200OK)]
        public IActionResult ReturnInfoOfOneWarehouse(Guid warehouseGuid)
        {
            var result = _warehouseService.GetWarehouseInfo(warehouseGuid);
            return Ok(result);
        }

        // Return list of all Warehouses
        // GET: <WarehousesController>/warehouses
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<WarehouseEntity>), StatusCodes.Status200OK)]
        public IActionResult ReturnListOfAllWarehouses()
        {
            var response = _warehouseService.GetWarehouses();

            return Ok(response);
        }

        // // POST <WarehousesController>
        // [HttpPost]
        // [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        // public async Task<IActionResult> Post([FromBody] WarehouseEntity warehouseEntity)
        // {
        //     var result = await _warehouseService.AddWarehouse(warehouseEntity);
        //     return Ok(result);
        // }
        //
        // // DELETE <WarehousesController>/5
        // [HttpDelete("{id}")]
        // [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        // public async Task<IActionResult> Delete(Guid id)
        // {
        //     var result = await _warehouseService.RemoveWarehouse(id);
        //     return Ok(result);
        // }


        // Warehouse Items

        // Return list of all WarehouseItems
        // GET: <WarehouseItemsController>
        // [HttpGet]
        // [ProducesResponseType(typeof(IEnumerable<WarehouseItemEntity>), StatusCodes.Status200OK)]
        // public IActionResult Get()
        // {
        //     var response = _warehouseService.GetWarehouseItems();
        //
        //     return Ok(response);
        // }

        // POST <WarehouseItemsController>
        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public async Task<IActionResult> Post([FromBody] WarehouseItemEntity warehouseItemEntity)
        {
            var result = await _warehouseService.AddWarehouseItem(warehouseItemEntity);
            return Ok(result);
        }
    }
}