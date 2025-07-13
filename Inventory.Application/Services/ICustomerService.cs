using Inventory.Application.DTOs;
using Inventory.Domain.Entities;

namespace Inventory.Application.Services
{
    public interface ICustomerService
    {
        Task<PagedResult<Customer>> GetPagedCustomersAsync(int pageNumber, int pageSize, string search);
        Task<Customer?> GetCustomerByIdAsync(int customerId);
        Task AddCustomerAsync(Customer customer);
        Task UpdateCustomerAsync(Customer customer);
        Task DeleteCustomerAsync(int customerId);
    }

}
