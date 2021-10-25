using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DistributedWarehouses.Domain;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Dto;
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

        // GET: api/<ItemController>/$SKU
        [HttpGet("{SKU}")]
        [ProducesResponseType(typeof(ItemDto), StatusCodes.Status200OK)]
        public IActionResult Get(string SKU)
        {
            //TODO: perkelti i service
            var item = _itemsRepository.GetItem(SKU);
            var result = new ItemDto
            {
                SKU = item.SKU,
                Title = item.Title,
                InWarehouses = _itemsRepository.GetItemsInWarehouses(SKU)
            };
            return Ok(result);
        }

        // POST api/<ItemController>
        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public async Task<IActionResult> Post([FromBody] Item item)
        {
            var result = await _itemsRepository.AddItem(item);
            return Ok(result);
        }

        // DELETE api/<ItemController>/$SKU
        [HttpDelete("{SKU}")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(string SKU)
        {
            var result = await _itemsRepository.RemoveItem(SKU);
            return Ok(result);
        }
        
        ////
        //// // PUT api/<ItemController>/5
        //// [HttpPut("{id}")]
        //// public void Put(int id, [FromBody] string value)
        //// {
        //// }
        ////

    }
}
