using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DistributedWarehouses.ApplicationServices;
using DistributedWarehouses.Domain;
using DistributedWarehouses.Domain.Entities;
using Microsoft.AspNetCore.Http;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DistributedWarehouses.Api.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        private readonly IWarehouseService _warehouseService;

        public WarehouseController(IWarehouseService warehouseService)
        {
            _warehouseService = warehouseService;
        }

        // Return list of all Warehouses
        // GET: api/<WarehouseController>/warehouses
        [HttpGet("warehouses")]
        [ProducesResponseType(typeof(IEnumerable<WarehouseEntity>), StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            var response = _warehouseService.GetWarehouses();

            return Ok(response);
        }

        // GET api/<WarehouseController>/5
        [HttpGet("warehouses/{id}")]
        [ProducesResponseType(typeof(WarehouseEntity), StatusCodes.Status200OK)]
        public IActionResult Get(Guid id)
        {
            var result = _warehouseService.GetWarehouse(id);
            return Ok(result);
        }

        // POST api/<WarehouseController>
        [HttpPost("warehouses")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public async Task<IActionResult> Post([FromBody] WarehouseEntity warehouseEntity)
        {
            var result = await _warehouseService.AddWarehouse(warehouseEntity);
            return Ok(result);
        }

        // DELETE api/<WarehouseController>/5
        [HttpDelete("warehouses/{id}")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _warehouseService.RemoveWarehouse(id);
            return Ok(result);
        }
    }
}