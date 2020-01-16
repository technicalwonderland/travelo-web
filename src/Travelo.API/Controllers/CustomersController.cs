using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Travelo.Core.Domain.DTO;
using Travelo.Core.Services;

namespace Travelo.API.Controllers
{
    [Route("api/[controller]")]
    public class CustomersController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
            => Json(await _customerService.GetCustomersAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
            => Json(await _customerService.GetCustomerAsync(id));

        [HttpGet("fullname/{name}")]
        public async Task<IActionResult> GetByFullNameAsync(string name)
            => Json(await _customerService.GetCustomersByFullNameAsync(name));

        [HttpPost()]
        public async Task<IActionResult> PostAsync([FromBody] CustomerDTO customer)
        {
            var id = await _customerService.AddCustomerAsync(customer.FirstName, customer.LastName);
            return Created($"customers/{id}", null);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(Guid id, [FromBody] CustomerDTO customer)
        {
            await _customerService.EditCustomerAsync(id, customer.FirstName, customer.LastName);
            return Ok();
        }
    }
}