using Inventory.Application.DTOs;
using Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Services
{
    public interface ISaleService
    {
        Task<Sale> ProcessSaleAsync(Sale sale);
        Task<SaleReportDto> GetSalesSummaryAsync(DateTime startDate, DateTime endDate);
    }

}
