using Inventory.Application.DTOs;
using Inventory.Application.Services;
using Inventory.Domain.Entities;
using Inventory.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService;
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
                var result = await _productService.GetPagedProductsAsync(pageNumber, pageSize, search);
                return Ok(ApiResponse<PagedResult<Product>>.Success(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching product list");
                return StatusCode(500, ApiResponse<string>.Fail("Something went wrong", 500));
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product == null)
                    return NotFound(ApiResponse<Product>.Fail("Product not found", 404));

                return Ok(ApiResponse<Product>.Success(product));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching product by ID");
                return StatusCode(500, ApiResponse<string>.Fail("Something went wrong", 500));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Product product)
        {
            try
            {
                await _productService.AddProductAsync(product);
                return CreatedAtAction(nameof(GetById), new { id = product.ProductId },
                    ApiResponse<Product>.Success(product, "Product created", 201));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding product");
                return StatusCode(500, ApiResponse<string>.Fail("Failed to create product", 500));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Product product)
        {
            try
            {
                product.ProductId = id;
                await _productService.UpdateProductAsync(product);
                return Ok(ApiResponse<Product>.Success(product, "Product updated"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product");
                return StatusCode(500, ApiResponse<string>.Fail("Failed to update product", 500));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _productService.DeleteProductAsync(id);
                return Ok(ApiResponse<string>.Success(null, "Product deleted"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting product");
                return StatusCode(500, ApiResponse<string>.Fail("Failed to delete product", 500));
            }
        }
    }
}
