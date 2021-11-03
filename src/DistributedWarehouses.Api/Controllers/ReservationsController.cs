using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using DistributedWarehouses.Domain.Entities;
using DistributedWarehouses.Domain.Services;
using DistributedWarehouses.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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

        // Remove Reservation of SKU
        // DELETE api/<ReservationController>/{item}/{warehouse}/{reservation}
        [HttpDelete("{reservationId:guid}/items/{itemSku:regex(^[[a-zA-Z0-9]]*$)}/warehouses/{warehouseId:guid}/")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public IActionResult RemoveSKUReservation(string itemSku, Guid warehouseId, Guid reservationId)
        {
            var result = _reservationService.RemoveReservationItem(itemSku, warehouseId, reservationId);
            return Ok(result);
        }

        //// GET api/<ReservationsController>/5
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("{id:guid}", Name = "GetValueById")]
        [ProducesResponseType(typeof(WarehouseEntity), StatusCodes.Status200OK)]
        public IActionResult GetReservation(Guid id)
        {
            var result = _reservationService.GetReservation(id);
            return Ok(result);
        }

        // POST api/<ReservationsController>
        [HttpPost]
        [ProducesResponseType(typeof(ReservationIdDto), StatusCodes.Status201Created)]
        public IActionResult AddItemReservation([FromBody] ReservationInputDto reservationInputDto)
        {
            var result = _reservationService.AddReservation(reservationInputDto);
            return Created(Url.Link("GetValueById", new { id = result.ReservationId }), result);
        }
    }
}