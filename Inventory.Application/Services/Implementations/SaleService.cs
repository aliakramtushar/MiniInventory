using Inventory.Application.DTOs;
using Inventory.Application.Repositories;
using Inventory.Application.Repositories.Implementations;
using Inventory.Domain.Entities;
using Inventory.Infrastructure.Persistence;


namespace Inventory.Application.Services.Implementations
{
    public class SaleService : ISaleService
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly AppDbContext _context;

        private static readonly SemaphoreSlim _concurrencyLimit = new(3, 3); // Max 3 concurrent sales


        public SaleService(ISaleRepository saleRepository, IProductRepository productRepository, ICustomerRepository customerRepository, AppDbContext context)
        {
            _saleRepository = saleRepository;
            _productRepository = productRepository;
            _customerRepository = customerRepository;
            _context = context;
        }

        public async Task<Sale> ProcessSaleAsync(Sale sale)
        {
            if (sale.DiscountPercent < 0 || sale.DiscountPercent > 100)
            {
                throw new ArgumentException("Discount percent must be between 0 and 100.");
            }

            if (!await _concurrencyLimit.WaitAsync(0)) // Non-blocking check
            {
                throw new InvalidOperationException("Too many concurrent sales. Try again later.");
            }

            try
            {
                using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    if (sale.CustomerId!= null)
                    {
                        var customer = await _customerRepository.GetByIdAsync(sale.CustomerId.Value);
                        if (customer == null || customer.IsDeleted == true)
                        {
                            throw new KeyNotFoundException($"There is no customer with this id");
                        }
                    }

                    // Validate stock and update quantities
                    foreach (var detail in sale.SaleDetails)
                    {
                        var product = await _productRepository.GetByIdAsync(detail.ProductId);
                        if (product == null || product.StockQty < detail.Quantity)
                        {
                            throw new InvalidOperationException($"Insufficient stock for product ID {detail.ProductId}.");
                        }

                        product.StockQty -= detail.Quantity;
                        await _productRepository.UpdateAsync(product);
                    }

                    // ⏱ Simulate processing delay
                    await Task.Delay(3000);

                    // Calculate totals
                    decimal totalAmount = sale.SaleDetails.Sum(d => d.Price * d.Quantity);
                    decimal discountAmount = totalAmount * (sale.DiscountPercent / 100);
                    decimal amountAfterDiscount = totalAmount - discountAmount;
                    decimal vatAmount = amountAfterDiscount * 0.15m;
                    decimal finalTotal = amountAfterDiscount + vatAmount;

                    // Set calculated values
                    sale.TotalAmount = totalAmount;
                    sale.DiscountAmount = discountAmount;
                    sale.VatAmount = vatAmount;
                    sale.FinalTotal = finalTotal;

                    var createdSale = await _saleRepository.CreateSaleAsync(sale);

                    await transaction.CommitAsync();
                    return createdSale;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            finally
            {
                _concurrencyLimit.Release(); // Always release
            }
        }

        public async Task<SaleReportDto> GetSalesSummaryAsync(DateTime startDate, DateTime endDate)
        {
            var sales = await _saleRepository.GetSalesSummaryAsync(startDate, endDate);
            return MapToSaleReportDto(sales);
        }

        private SaleReportDto MapToSaleReportDto(IEnumerable<Sale> sales)
        {
            return new SaleReportDto
            {
                TotalSales = sales.Sum(s => s.TotalAmount),
                TotalRevenue = sales.Sum(s => s.PaidAmount),
                TransactionCount = sales.Count()
            };
        }
    }

}
