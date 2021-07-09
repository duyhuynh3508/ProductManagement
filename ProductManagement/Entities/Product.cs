using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductManagement.Entities
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public Category Category { get; set; }
        public int CategoryID { get; set; }
        public int Amount { get; set; }
        public double UnitPrice { get; set; }
    }
}
