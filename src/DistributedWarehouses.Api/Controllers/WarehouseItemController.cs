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
    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseItemController : ControllerBase
    {
        private readonly IWarehouseItemRepository _warehouseItemRepository;

        public WarehouseItemController(IWarehouseItemRepository warehouseItemRepository)
        {
            _warehouseItemRepository = warehouseItemRepository;
        }

        // GET: api/<WarehouseItemController>
        [HttpGet]
        public IEnumerable<WarehouseItem> Get()
        {
            return _warehouseItemRepository.GetWarehouseItems();
        }

        // POST api/<WarehouseItemController>
        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public async Task<IActionResult> Post([FromBody] Item item)
        {
            var result = await _warehouseItemRepository.AddWarehouseItem(item);
            return Ok(result);
        }


        // // GET api/<WarehouseItemController>/5
        // [HttpGet("{id}")]
        // public string Get(int id)
        // {
        //     return "value";
        // }
        //

        //
        // // PUT api/<WarehouseItemController>/5
        // [HttpPut("{id}")]
        // public void Put(int id, [FromBody] string value)
        // {
        // }
        //
        // // DELETE api/<WarehouseItemController>/5
        // [HttpDelete("{id}")]
        // public void Delete(int id)
        // {
        // }
    }
}
