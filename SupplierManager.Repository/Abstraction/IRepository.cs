using SupplierManager.Repository.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupplierManager.Repository.Abstraction
{
    public interface IRepository
    {
		public Task<Supplier> GetSupplierById(int customerId, CancellationToken ct = default);
		public Task<Supplier> CreateSupplierAsync(string certifiedEmail, string companyName, string email, string phone, string taxCode, string VatNumber, CancellationToken ct = default);
		public Task DeleteSupplier(Supplier supplierId, CancellationToken ct = default);
		public Task SaveChanges(CancellationToken ct = default);
		public Task<Supplier> UpdateSupplierAsync(int supplierId, string certifiedEmail, string companyName, string email, string phone, string taxCode, string VatNumber, CancellationToken ct = default);
		public Task DeleteProduct (Product productId, CancellationToken ct = default);
		public Task<Product> GetProductById(int productId, CancellationToken ct = default);
		public Task<Product> CreateProductAsync(int SupplierProductCode, decimal price, int minQuantity, int supplierId, CancellationToken ct = default);
		public Task<Product> UpdateProductAsync(int productId, int supplierProductCode, decimal price, int minQuantity, CancellationToken ct = default);
		public Task<Order> CreateOrderAsync(DateTime delivery, int supplierId, CancellationToken ct = default);
		public Task<ProductOrder> CreateProductOrderAsync(int quantity, int discount, int productId, int OrderId, CancellationToken ct = default);
		public Task<Order?> GetOrderByIdAsync(int OrderId, CancellationToken ct = default);
		public Task<List<Order>?> GetOrderBySupplierIdAsync(int supplierId, CancellationToken ct = default);
		public Task DeleteOrderAsync(Order orderId, CancellationToken ct = default);
		public Task DeleteProductOrder(ProductOrder productOrder, CancellationToken ct = default);
		public Task<List<Product>> GetAllProductBySupplierId(int productId, CancellationToken ct = default);

	}
}
