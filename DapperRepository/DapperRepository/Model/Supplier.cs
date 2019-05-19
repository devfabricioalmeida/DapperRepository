using System.Collections.Generic;

namespace DapperRepository.Model
{
    public class Supplier : Entity<Supplier>
    {
        public string Name { get; set; }
        public List<Product> Products { get; set; }
    }
}
