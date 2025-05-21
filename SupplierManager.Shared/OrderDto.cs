using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupplierManager.Shared
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime DeliveryDate { get; set; }
		public List<ProductOrderDto> ProductOrder { get; set; }
		public int SupplierId { get; set; }

    }
}
