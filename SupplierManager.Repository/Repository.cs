using Microsoft.EntityFrameworkCore;
using SupplierManager.Repository.Abstraction;
using SupplierManager.Repository.Model;
using System.Linq;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using System.Numerics;
using Microsoft.EntityFrameworkCore.Storage;

namespace SupplierManager.Repository
{
	public class Repository(SupplierDbContext dbContext) : IRepository
	{

		#region Order
		public async Task<Order?> CreateOrderAsync(Order model, CancellationToken ct = default)
		{
			await dbContext.Orders.AddAsync(model, ct);
			return model;
		}
		public async Task DeleteOrderAsync(Order order, CancellationToken ct = default)
		{

			dbContext.Orders.Remove(order);
		}


		public async Task<Order?> GetOrderByIdAsync(int OrderId, CancellationToken ct = default)
		{
			return  await dbContext.Orders
				.Where(o => o.Id == OrderId)
				.Include(o => o.ProductOrder)
				.ThenInclude(p => p.Product)
				.AsNoTracking()
				.SingleOrDefaultAsync(ct);
		}
		public  Task<List<Order>>? GetOrderBySupplierIdAsync(int supplierId, CancellationToken ct = default)
		{
			return  dbContext.Orders
				.Where(o => o.SupplierId == supplierId)
				.Include(o => o.ProductOrder)
					.ThenInclude(po => po.Product)
				.AsNoTracking()
				.ToListAsync(ct);

		}
		public async  Task DeleteAllOrderBySupplierIdAsync(List<Order> orderList, CancellationToken ct = default)
		{
			dbContext.Orders.RemoveRange(orderList);
				
		}

		#endregion

		#region Supplier
		public async Task<Supplier> CreateSupplierAsync( Supplier model, CancellationToken ct = default)
		{
			await dbContext.AddAsync(model, ct);
			return model;
		}
		public async Task DeleteSupplier(Supplier supplier, CancellationToken ct = default)
		{
			dbContext.Suppliers.Remove(supplier);
		}
		public async Task<Supplier?> GetSupplierById(int customerId, CancellationToken ct = default)
		{
			return await dbContext.Suppliers
				.Where(s => s.Id == customerId)
				.Include(s => s.Products)
				.Include(s => s.Orders)
					.ThenInclude(o => o.ProductOrder)  
				.AsNoTracking()  
				.SingleOrDefaultAsync(ct);

		}
		public async Task<Supplier?> UpdateSupplierAsync(Supplier model, CancellationToken ct = default)
		{
			Supplier? supplier = await GetSupplierById(model.Id, ct);
			if (supplier == null) return null;
			dbContext.Suppliers.Update(model);
			return model;
		}
		#endregion

		#region Product
		public async Task<Product> CreateProductAsync(Product model, CancellationToken ct = default)
		{
			await dbContext.AddAsync(model, ct);
			return model;
		}
		public async Task DeleteProduct(int productId, CancellationToken ct = default)
		{
			var model = await GetProductById(productId, ct);
			if(model == null) return;
			dbContext.Products.Remove(model);
		}
		public Task<List<Product>> GetAllProductBySupplierId(int supplierId, CancellationToken ct = default)
		{
			return dbContext.Products.Where(o => o.SupplierId == supplierId).ToListAsync(ct);
		}
		public async Task<Product?> GetProductById(int productId, CancellationToken ct = default)
		{
			return await dbContext.Products.Where(p => p.Id == productId).SingleOrDefaultAsync(ct);
		}
		public async Task<Product?> UpdateProductAsync(Product model, CancellationToken ct = default)
		{
			Product? product = await GetProductById(model.Id, ct);
			if (product == null) return null;
			dbContext.Products.Update(model);
			return model;
		}
		public async Task DeleteAllProductsBySupplierIdAsync(List<Product> productList, CancellationToken ct = default)
		{
			dbContext.Products.RemoveRange(productList);
		}
		#endregion

		#region ProductOrder
		public async Task<ProductOrder> CreateProductOrderAsync(ProductOrder model, CancellationToken ct = default)
		{
			await dbContext.ProductOrders.AddAsync(model);
			return model;			
		}
		public async Task DeleteProductOrder(ProductOrder productOrder, CancellationToken ct = default)
		{
			dbContext.ProductOrders.Remove(productOrder);
		}
		#endregion

		public async Task SaveChanges(CancellationToken ct = default)
		{
			await dbContext.SaveChangesAsync(ct);
		}
		public async Task CreateTransaction(Func<Task> action)
		{
			if (dbContext.Database.CurrentTransaction != null)
			{
				await action();
			}
			else
			{
				using var transaction = await dbContext.Database.BeginTransactionAsync();
				try
				{
					await action();
					await transaction.CommitAsync();
				}
				catch
				{
					await transaction.RollbackAsync();
					throw;
				}
			}
		}



	}
}
