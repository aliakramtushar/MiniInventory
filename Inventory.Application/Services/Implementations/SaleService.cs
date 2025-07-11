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
    public class SaleService : ISaleService
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IProductRepository _productRepository;

        private static readonly SemaphoreSlim _concurrencyLimit = new(3, 3);

        public SaleService(ISaleRepository saleRepository, IProductRepository productRepository)
        {
            _saleRepository = saleRepository;
            _productRepository = productRepository;
        }

        public async Task<Sale> ProcessSaleAsync(Sale sale)
        {
            // Concurrency control
            if (!await _concurrencyLimit.WaitAsync(0))
            {
                throw new InvalidOperationException("Too many concurrent sales. Try again later.");
            }

            try
            {
                foreach (var detail in sale.SaleDetails)
                {
                    var product = await _productRepository.GetByIdAsync(detail.ProductId);
                    if (product == null || product.StockQty < detail.Quantity)
                    {
                        throw new InvalidOperationException($"Insufficient stock for product ID {detail.ProductId}.");
                    }

                    // Reduce stock
                    product.StockQty -= detail.Quantity;
                    await _productRepository.UpdateAsync(product);
                }

                // Simulate processing delay
                await Task.Delay(3000);

                // Save sale
                return await _saleRepository.CreateSaleAsync(sale);
            }
            finally
            {
                _concurrencyLimit.Release();
            }
        }

        public async Task<SaleReportDto> GetSalesSummaryAsync(DateTime startDate, DateTime endDate)
        {
            var sales = await _saleRepository.GetSalesSummaryAsync(startDate, endDate);

            return new SaleReportDto
            {
                //TotalSales = sales.Sum(s => s.TotalAmount),
                //TotalRevenue = sales.Sum(s => s.PaidAmount),
                //TransactionCount = sales.Count()
            };
        }
    }

}
