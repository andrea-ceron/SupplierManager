using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupplierManager.Shared
{
    public class CreateOrUpdateSupplierDto
    {
        public SupplierDto supplier { get; set; }
        public List<ProductDto> products { get; set; }
	}
}
