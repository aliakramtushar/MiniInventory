using Inventory.Application.DTOs;
using Inventory.Domain.Entities;

namespace Inventory.Application.Repositories
{
    public interface ICustomerRepository
    {
        Task<PagedResult<Customer>> ListPagedAsync(int pageNumber, int pageSize, string search);
        Task<Customer?> GetByIdAsync(int customerId);
        Task AddAsync(Customer customer);
        Task UpdateAsync(Customer customer);
        Task DeleteAsync(int customerId);
    }

}
