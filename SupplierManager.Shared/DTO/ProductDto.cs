using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupplierManager.Shared.DTO
{
    public class CreateProductDto
	{
        public int SupplierProductCode { get; set; }
        public decimal Price { get; set; }
        public int MinQuantity { get; set; }
		public int? SupplierId { get; set; }


	}

	public class CreateProductFromSupplierControllerDto
	{
		public int SupplierProductCode { get; set; }
		public decimal Price { get; set; }
		public int MinQuantity { get; set; }

	}

	public class ReadProductDto
	{
		public int Id { get; set; }
		public int SupplierProductCode { get; set; }
		public decimal Price { get; set; }
		public int MinQuantity { get; set; }
		public List<ReadProductOrderDto> ProductOrders { get; set; }
		public int SupplierId { get; set; }
	}


	public class UpdateProductDto
	{
		public int? Id { get; set; }
		public int? SupplierProductCode { get; set; }
		public decimal? Price { get; set; }
		public int? MinQuantity { get; set; }
		public int SupplierId { get; set; } 

	}
}
