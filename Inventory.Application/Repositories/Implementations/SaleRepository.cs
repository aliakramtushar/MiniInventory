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
    public class SaleRepository : ISaleRepository
    {
        private readonly AppDbContext _context;

        public SaleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Sale> CreateSaleAsync(Sale sale)
        {
            foreach (var detail in sale.SaleDetails)
            {
                var product = await _context.Products.FindAsync(detail.ProductId);
                if (product == null || product.StockQty < detail.Quantity)
                {
                    throw new InvalidOperationException("Insufficient stock.");
                }

                product.StockQty -= detail.Quantity;
            }

            await Task.Delay(3000); // Simulate processing delay
            _context.Sales.Add(sale);
            await _context.SaveChangesAsync();

            return sale;
        }

        public async Task<IEnumerable<Sale>> GetSalesSummaryAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Sales
                .Where(s => s.SaleDate >= startDate && s.SaleDate <= endDate)
                .Include(s => s.SaleDetails)
                .ToListAsync();
        }
    }

}
