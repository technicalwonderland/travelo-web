using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Travelo.Core.Domain.DTO;
using Travelo.Core.Services;

namespace Travelo.API.Controllers
{
    [Route("api/[controller]")]
    public class TripsController : Controller
    {
        private readonly ITripService _tripService;

        public TripsController(ITripService tripService)
        {
            _tripService = tripService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
            => Json(await _tripService.GetTripsAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GatAsync(Guid id)
            => Json( await _tripService.GetTripAsync(id));

        [HttpPost]
        public async Task<IActionResult> PostTripAsync([FromBody] TripDTO tripDto)
        {
            var id = await _tripService.AddTripAsync(tripDto);
            return Created($"trips/{id}", null);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTripAsync(Guid id, [FromBody] TripDTO tripDto)
        {
            await _tripService.EditTripAsync(id, tripDto);
            return Ok();
            
        }

        [HttpPost("{tripId}/customers/{customerId}")]
        public async Task<IActionResult> PostCustomerToTripAsync(Guid tripId, Guid customerId)
        {
            await _tripService.AddCustomerToTripAsync(tripId, customerId);
            return Ok();
        }
        
    }
}