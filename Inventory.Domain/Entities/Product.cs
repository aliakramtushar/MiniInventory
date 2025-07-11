using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Entities
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Barcode { get; set; }
        public decimal Price { get; set; }
        public decimal StockQty { get; set; }
        public string Category { get; set; }
        public bool IsActive { get; set; } = true; // Status
        public bool IsDeleted { get; set; } = false; // Soft delete
    }
}