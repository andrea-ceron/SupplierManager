using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupplierManager.Repository.Model
{
    public class Order
    {
		public int Id { get; set; }
		public DateTime DeliveryDate { get; set; }
		public List<ProductOrder>? ProductOrder { get; set; }
		public int SupplierId { get; set; }
		public Supplier Supplier { get; set; }
	}
}
 