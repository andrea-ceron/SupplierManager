using Microsoft.EntityFrameworkCore;
using SupplierManager.Repository.Abstraction;
using SupplierManager.Repository.Model;
using System.Linq;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using System.Numerics;

namespace SupplierManager.Repository
{
	public class Repository(SupplierDbContext dbContext) : IRepository
	{


		public async Task<Order?> CreateOrderAsync(DateTime delivery, int supplierId, CancellationToken ct = default)
		{
			Order NewOrder = new()
			{
				DeliveryDate = delivery,
				SupplierId = supplierId,
			};
			await dbContext.Orders.AddAsync(NewOrder, ct);
			return NewOrder;
		}

		public async Task<Product> CreateProductAsync(int SupplierProductCode, decimal price, int minQuantity, int supplierId, CancellationToken ct = default)
		{
			Product product = new()
			{
				SupplierProductCode = SupplierProductCode,
				Price = price,
				MinQuantity = minQuantity,
				SupplierId = supplierId,
			};
			await dbContext.AddAsync(product, ct);
			return product;
		}

		public async Task<ProductOrder> CreateProductOrderAsync(int quantity, int discount, int productId, int OrderId, CancellationToken ct = default)
		{
			
			ProductOrder newproductOrder = new ()
			{
				Quantity = quantity,
				Discount = discount,
				ProductId = productId,
				OrderId = OrderId

			};
			return newproductOrder;
			
		}

		public async Task<Supplier> CreateSupplierAsync( string certifiedEmail, string companyName, string email, string phone, string taxCode, string VatNumber, CancellationToken ct = default)
		{
			Supplier supplier = new()
			{
				CertifiedEmail = certifiedEmail,
				CompanyName = companyName,
				Email = email,
				Phone = phone,
				TaxCode = taxCode,
				VATNumber = VatNumber
			};
			await dbContext.AddAsync(supplier, ct);
			return supplier;
		}

		public async Task DeleteOrderAsync(Order order, CancellationToken ct = default)
		{

			dbContext.Orders.Remove(order);
		}

		public async Task DeleteProduct(Product product, CancellationToken ct = default)
		{
			dbContext.Products.Remove(product);
		}

		public async Task DeleteProductOrder(ProductOrder productOrder, CancellationToken ct = default)
		{
			dbContext.ProductOrders.Remove(productOrder);
		}

		public async Task DeleteSupplier(Supplier supplier, CancellationToken ct = default)
		{
			dbContext.Suppliers.Remove(supplier);
		}

		public Task<List<Product>> GetAllProductBySupplierId(int supplierId, CancellationToken ct = default)
		{
			return dbContext.Products.Where(o => o.SupplierId == supplierId).ToListAsync(ct);
		}

		public async Task<Order?> GetOrderByIdAsync(int OrderId, CancellationToken ct = default)
		{
			return await dbContext.Orders
				.Where( o => o.Id == OrderId)
				.Include(o => o.ProductOrder)
				.ThenInclude(po => po.Product) 
				.SingleOrDefaultAsync(ct);
		}

		public  Task<List<Order>?> GetOrderBySupplierIdAsync(int supplierId, CancellationToken ct = default)
		{
			return  dbContext.Orders.Where(o => o.SupplierId == supplierId).ToListAsync(ct);

		}

		public async Task<Product?> GetProductById(int productId, CancellationToken ct = default)
		{
			return await dbContext.Products.Where(p => p.Id == productId).SingleOrDefaultAsync(ct);
		}

		public async Task<Supplier?> GetSupplierById(int customerId, CancellationToken ct = default)
		{
			return await dbContext.Suppliers.Where(s => s.Id == customerId).SingleOrDefaultAsync(ct);
		}

		public async Task SaveChanges(CancellationToken ct = default)
		{
			await dbContext.SaveChangesAsync(ct);
		}

		public async Task<Product?> UpdateProductAsync(int productId, int supplierProductCode, decimal price, int minQuantity, CancellationToken ct = default)
		{
			Product? newProduct = await GetProductById(productId, ct);
			if (newProduct == null) return null;

			newProduct.SupplierProductCode = supplierProductCode;
			newProduct.Price = price;
			newProduct.MinQuantity = minQuantity;

			return newProduct;
		}

		public async Task<Supplier?> UpdateSupplierAsync(int supplierId,string certifiedEmail, string companyName, string email, string phone, string taxCode, string VatNumber, CancellationToken ct = default)
		{
			Supplier? newSupplier = await GetSupplierById(supplierId, ct);
			if (newSupplier == null) return null;

			newSupplier.CompanyName = companyName;
			newSupplier.VATNumber = VatNumber;
			newSupplier.Email = email;
			newSupplier.TaxCode = taxCode;
			newSupplier.CertifiedEmail = certifiedEmail;
			newSupplier.Phone = phone;


			return newSupplier;
		}
	}
}
