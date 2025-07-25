﻿using Inventory.Application.DTOs;
using Inventory.Application.Repositories;
using Inventory.Application.Repositories.Implementations;
using Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Services.Implementations
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<PagedResult<Customer>> GetPagedCustomersAsync(int pageNumber, int pageSize, string search)
        {
            return await _customerRepository.ListPagedAsync(pageNumber, pageSize, search);
        }

        public async Task<Customer?> GetCustomerByIdAsync(int customerId)
        {
            return await _customerRepository.GetByIdAsync(customerId);
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            // Business logic like duplicate check, etc.
            await _customerRepository.AddAsync(customer);
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            await _customerRepository.UpdateAsync(customer);
        }

        public async Task DeleteCustomerAsync(int customerId)
        {
            await _customerRepository.DeleteAsync(customerId);
        }
    }

}
