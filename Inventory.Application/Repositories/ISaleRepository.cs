using Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Repositories
{
    public interface ISaleRepository
    {
        Task<Sale> CreateSaleAsync(Sale sale);
        Task<IEnumerable<Sale>> GetSalesSummaryAsync(DateTime startDate, DateTime endDate);
    }
}
