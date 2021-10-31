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
        [HttpDelete("{item:regex(^[[a-zA-Z0-9]]*$)}/{warehouse:guid}/{reservation:guid}")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public IActionResult RemoveSKUReservation(string item, Guid warehouse, Guid reservation)
        {
            var result = _reservationService.RemoveReservationItem(item, warehouse, reservation);
            return Ok(result);
        }

        // Return list of all Reservations
        // GET: <ReservationsController>/reservations
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ReservationEntity>), StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            var response = _reservationService.GetReservations();

            return Ok(response);
        }

        // GET api/<ReservationsController>/5
        [HttpGet("{id:guid}", Name = "GetValueById")]
        [ProducesResponseType(typeof(WarehouseEntity), StatusCodes.Status200OK)]
        public IActionResult Get(Guid id)
        {
            var result = _reservationService.GetReservation(id);
            return Ok(result);
        }

        // POST api/<ReservationsController>
        [HttpPost]
        [ProducesResponseType(typeof(ReservationIdDto), StatusCodes.Status201Created)]
        public IActionResult Post([FromBody] ReservationInputDto reservationInputDto)
        {
            var result = _reservationService.AddReservation(reservationInputDto);
            return Created(Url.Link("GetValueById", new { id = result.ReservationId }), result);
        }

        // // DELETE api/<ReservationsController>/5
        // [HttpDelete("{id}")]
        // [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        // public async Task<IActionResult> Delete(Guid id)
        // {
        //     var result = await _reservationService.RemoveReservation(id);
        //     return Ok(result);
        // }


        // Reservation Item

        // Return list of all ReservationItem
        // GET: <ReservationItemController>/ReservationItem
        // [HttpGet]
        // [ProducesResponseType(typeof(IEnumerable<ReservationItemEntity>), StatusCodes.Status200OK)]
        // public IActionResult Get()
        // {
        //     var response = _reservationService.GetReservationItems();
        //
        //     return Ok(response);
        // }

        // // GET api/<ReservationItemController>/5
        // [HttpGet("{id}")]
        // [ProducesResponseType(typeof(WarehouseEntity), StatusCodes.Status200OK)]
        // public IActionResult Get(Guid id)
        // {
        //     var result = _reservationService.GetReservationItem(id);
        //     return Ok(result);
        // }

        // // POST api/<ReservationItemController>
        // [HttpPost]
        // [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        // public async Task<IActionResult> Post([FromBody] ReservationItemEntity invoiceItemEntity)
        // {
        //     var result = await _reservationService.AddReservationItem(invoiceItemEntity);
        //     return Ok(result);
        // }

    }
}