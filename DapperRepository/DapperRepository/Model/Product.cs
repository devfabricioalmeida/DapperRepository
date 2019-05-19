using System;
using System.Collections.Generic;
using System.Text;

namespace DapperRepository.Model
{
    public class Product : Entity<Product>
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int SupplierId { get; set; }
    }
}
