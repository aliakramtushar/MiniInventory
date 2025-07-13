using Inventory.Application.DTOs;
using Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Repositories
{
    public interface IProductRepository
    {
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int productId);
        Task<PagedResult<Product>> ListPagedAsync(int pageNumber, int pageSize, string search);
        Task<Product> GetByIdAsync(int id);
    }
}
