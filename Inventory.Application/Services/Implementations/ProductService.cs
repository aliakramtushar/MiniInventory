using Inventory.Application.DTOs;
using Inventory.Application.Repositories;
using Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }


        public async Task<PagedResult<Product>> GetPagedProductsAsync(int pageNumber, int pageSize, string search)
        {
            return await _productRepository.ListPagedAsync(pageNumber, pageSize, search);
        }


        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            return await _productRepository.GetByIdAsync(productId);
        }

        public async Task AddProductAsync(Product product)
        {
            await _productRepository.AddAsync(product);
        }

        public async Task UpdateProductAsync(Product product)
        {
            await _productRepository.UpdateAsync(product);
        }

        public async Task DeleteProductAsync(int productId)
        {
            await _productRepository.DeleteAsync(productId);
        }
    }

}
