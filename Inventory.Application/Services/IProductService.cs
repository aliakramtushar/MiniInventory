using Inventory.Application.DTOs;
using Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Services
{
    public interface IProductService
    {
        Task<PagedResult<Product>> GetPagedProductsAsync(int pageNumber, int pageSize, string search);
        Task<Product?> GetProductByIdAsync(int productId);
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(int productId);
    }
}
