using Inventory.Application.Services;
using Inventory.Domain.Entities;
using Inventory.WebApi.Models; // for ApiResponse<T>
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SaleController : ControllerBase
    {
        private readonly ISaleService _saleService;
        private readonly ILogger<SaleController> _logger;

        public SaleController(ISaleService saleService, ILogger<SaleController> logger)
        {
            _saleService = saleService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSale([FromBody] Sale sale)
        {
            try
            {
                var result = await _saleService.ProcessSaleAsync(sale);
                return Ok(ApiResponse<Sale>.Success(result));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<string>.Fail(ex.Message, 400));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<string>.Fail(ex.Message, 404));
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("Too many concurrent sales"))
            {
                return StatusCode(429, ApiResponse<string>.Fail(ex.Message, 429));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing sale.");
                return StatusCode(500, ApiResponse<string>.Fail("Something went wrong.", 500));
            }
        }


        [HttpGet("summary")]
        public async Task<IActionResult> GetSalesSummary([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var summary = await _saleService.GetSalesSummaryAsync(startDate, endDate);
                return Ok(ApiResponse<object>.Success(summary, "Sales summary fetched"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching sales summary");
                return StatusCode(500, ApiResponse<string>.Fail("Failed to fetch sales summary", 500));
            }
        }
    }
}
