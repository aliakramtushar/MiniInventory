using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Entities
{
    public class Sale
    {
        public int SaleId { get; set; }
        public DateTime SaleDate { get; set; }
        public int? CustomerId { get; set; }

        public decimal TotalAmount { get; set; } 
        public decimal DiscountPercent { get; set; } 
        public decimal DiscountAmount { get; set; }
        public decimal VatAmount { get; set; }
        public decimal FinalTotal { get; set; }

        public decimal PaidAmount { get; set; }
        public decimal DueAmount { get; set; }

        public List<SaleDetail>? SaleDetails { get; set; }
    }
}

