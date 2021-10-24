using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DistributedWarehouses.Domain;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DistributedWarehouses.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemsRepository _itemsRepository;

        public ItemController(IItemsRepository itemsRepository)
        {
            _itemsRepository = itemsRepository;
        }

        // GET: api/<ItemController>
        [HttpGet]
        public IEnumerable<Item> Get()
        {
            return _itemsRepository.GetItems();
        }

        // GET api/<ItemController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ItemController>
        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public async Task<IActionResult> Post([FromBody] Item item)
        {
            var result = await _itemsRepository.AddItem(item);
            return Ok(result);
        }

        // PUT api/<ItemController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ItemController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
