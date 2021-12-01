using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace storefront.Models
{
    public class Product
    {
        public string ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double UnitPrice { get; set; }
        public double SellingPrice => (double)(UnitPrice * 1.2);
        public int? MaximumQuantity { get; set; }
    }
}
