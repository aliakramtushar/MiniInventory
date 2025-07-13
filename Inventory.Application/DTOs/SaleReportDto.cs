using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.DTOs
{
    public class SaleReportDto
    {
        public decimal TotalSales { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TransactionCount { get; set; }
    }
}
