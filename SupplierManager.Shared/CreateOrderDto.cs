using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupplierManager.Shared
{
    public class CreateOrderDto
    {
		public DateTime Delivery { get; set; }
		public int SupplierId { get; set; }
		public List<ProductOrderDto> ProductOrders { get; set; }
	}
}
