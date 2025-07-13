using Inventory.Domain.Entities;
using Inventory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

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
