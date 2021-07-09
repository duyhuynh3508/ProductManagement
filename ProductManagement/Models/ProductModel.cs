using ProductManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductManagement.Models
{
    public class ProductModel
    {
        public string ProductName { get; set; }
        public int CategoryId { get; set; }
        public int Amount { get; set; }
        public double UnitPrice { get; set; }
    }
}
