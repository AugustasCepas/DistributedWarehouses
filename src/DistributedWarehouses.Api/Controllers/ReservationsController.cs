using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Domain.Services;
using DistributedWarehouses.Dto;
using Microsoft.AspNetCore.Http;
using DistributedWarehouses.Api.Swagger;
using Swashbuckle.AspNetCore.Filters;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DistributedWarehouses.Api.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationsController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        /// <summary>
        /// 4) Remove Reservation of SKU
        /// </summary>
        [HttpDelete("{id}/items/{sku}")]
        [ProducesResponseType(typeof(ReservationRemovedDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> RemoveSKUReservation(string sku, Guid id)
        {
            var result =  await _reservationService.RemoveReservationItemAsync(sku, id);
            return Ok(result);
        }

        /// <summary>
        /// 3) Reserve SKU
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(IdDto), StatusCodes.Status201Created)]
        [SwaggerRequestExample(typeof(ReservationInputDto), typeof(ReservationInputDtoExample))]
        public async Task<IActionResult> AddItemReservation([FromBody] ReservationInputDto reservationInputDto)
        {
            var result = await _reservationService.AddReservationAsync(reservationInputDto);
            var link = Url.Link("GetValueById", new { id = result.Id });
            return Created(link, result);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("{id:guid}", Name = "GetValueById")]
        [ProducesResponseType(typeof(WarehouseEntity), StatusCodes.Status200OK)]
        public IActionResult GetReservation(Guid id)
        {
            var result = _reservationService.GetReservation(id);
            return Ok(result);
        }
    }
}