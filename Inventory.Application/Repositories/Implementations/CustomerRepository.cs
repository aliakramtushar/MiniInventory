using Inventory.Application.DTOs;
using Inventory.Domain.Entities;
using Inventory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Repositories.Implementations
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _context;

        public CustomerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Customer>> ListPagedAsync(int pageNumber, int pageSize, string search)
        {
            var query = _context.Customers.Where(p => p.IsDeleted != true).AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchLower = search.ToLower();
                query = query.Where(p => p.FullName.ToLower().Contains(searchLower));
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Customer>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<Customer?> GetByIdAsync(int customerId)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);
        }

        public async Task AddAsync(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Customer customer)
        {
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int customerId)
        {
            var customer = await GetByIdAsync(customerId);
            if (customer != null)
            {
                customer.IsDeleted = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}
