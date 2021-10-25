using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DistributedWarehouses.Domain;
using DistributedWarehouses.Domain.Entities;
using Microsoft.AspNetCore.Http;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DistributedWarehouses.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        private readonly IWarehouseRepository _warehouseRepository;


        public WarehouseController(IWarehouseRepository warehouseRepository)
        {
            _warehouseRepository = warehouseRepository;
        }

        // GET: api/<WarehouseController>
        [HttpGet]
        public IEnumerable<Warehouse> Get()
        {
            return _warehouseRepository.GetWarehouses();
        }

        // GET api/<WarehouseController>/5
        [HttpGet("{id}")]
        public IEnumerable<Warehouse> Get(Guid id)
        {
            return _warehouseRepository.GetWarehouse(id);
        }

        // POST api/<WarehouseController>
        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public async Task<IActionResult> Post([FromBody] Warehouse warehouse)
        {
            var result = await _warehouseRepository.AddWarehouse(warehouse);
            return Ok(result);
        }

        // DELETE api/<WarehouseController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _warehouseRepository.RemoveWarehouse(id);
            return Ok(result);
        }

        //// // PUT api/<WarehouseController>/5
        //// [HttpPut("{id}")]
        //// public void Put(int id, [FromBody] string value)
        //// {
        //// }
        ////

    }
}