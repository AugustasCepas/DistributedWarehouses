using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DistributedWarehouses.ApplicationServices;
using DistributedWarehouses.Domain.Entities;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DistributedWarehouses.Api.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationsController(
            IReservationService reservationService)
        {
            _reservationService = reservationService;
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
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(WarehouseEntity), StatusCodes.Status200OK)]
        public IActionResult Get(Guid id)
        {
            var result = _reservationService.GetReservation(id);
            return Ok(result);
        }

        // POST api/<ReservationsController>
        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public async Task<IActionResult> Post([FromBody] ReservationEntity reservationEntity)
        {
            var result = await _reservationService.AddReservation(reservationEntity);
            return Ok(result);
        }

        // DELETE api/<ReservationsController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _reservationService.RemoveReservation(id);
            return Ok(result);
        }
    }
}