using Inventory.Application.DTOs;
using Inventory.Application.Services;
using Inventory.Application.Services.Implementations;
using Inventory.Domain.Entities;
using Inventory.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ICustomerService customerService, ILogger<CustomerController> logger)
        {
            _customerService = customerService;
            _logger = logger;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null)
        {
            try
            {
                var result = await _customerService.GetPagedCustomersAsync(pageNumber, pageSize, search);
                return Ok(ApiResponse<PagedResult<Customer>>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving customer list");
                return StatusCode(500, ApiResponse<string>.Fail("Something went wrong", 500));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var customer = await _customerService.GetCustomerByIdAsync(id);
                if (customer == null)
                    return NotFound(ApiResponse<Customer>.Fail("Customer not found", 404));

                return Ok(ApiResponse<Customer>.Success(customer));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving customer by ID");
                return StatusCode(500, ApiResponse<string>.Fail("Something went wrong", 500));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Customer customer)
        {
            try
            {
                await _customerService.AddCustomerAsync(customer);
                return CreatedAtAction(nameof(GetById), new { id = customer.CustomerId },
                    ApiResponse<Customer>.Success(customer, "Customer created", 201));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding customer");
                return StatusCode(500, ApiResponse<string>.Fail("Failed to create customer", 500));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Customer customer)
        {
            try
            {
                customer.CustomerId = id;
                await _customerService.UpdateCustomerAsync(customer);
                return Ok(ApiResponse<Customer>.Success(customer, "Customer updated"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating customer");
                return StatusCode(500, ApiResponse<string>.Fail("Failed to update customer", 500));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _customerService.DeleteCustomerAsync(id);
                return Ok(ApiResponse<string>.Success(null, "Customer deleted"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting customer");
                return StatusCode(500, ApiResponse<string>.Fail("Failed to delete customer", 500));
            }
        }
    }
}
