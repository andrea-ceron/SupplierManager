using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupplierManager.Shared
{
    public class ProductDto
    {
        public int Id { get; set; }
        public int SupplierProductCode { get; set; }
        public decimal Price { get; set; }
        public int MinQuantity { get; set; }
        public int State { get; set; }

    }
}
