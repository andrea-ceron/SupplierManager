﻿using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
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
		#region Supplier
		public Task<Supplier> GetSupplierById(int customerId, CancellationToken ct = default);
		public Task<Supplier> CreateSupplierAsync(Supplier model, CancellationToken ct = default);
		public Task DeleteSupplier(int supplierId, CancellationToken ct = default);
		public Task<Supplier> UpdateSupplierAsync(Supplier model, CancellationToken ct = default);
		#endregion


		#region Order
		public Task<Order> CreateOrderAsync(Order model, CancellationToken ct = default);
		public Task<Order?> GetOrderByIdAsync(int OrderId, CancellationToken ct = default);
		public Task<List<Order>>? GetOrderBySupplierIdAsync(int supplierId, CancellationToken ct = default);
		public Task DeleteOrderAsync(int orderId, CancellationToken ct = default);
		public Task DeleteAllOrdersBySupplierIdAsync(int supplierId, CancellationToken ct = default);

		#endregion

		#region Product
		public Task DeleteProduct (int productId, CancellationToken ct = default);
		public Task<Product> GetProductById(int productId, CancellationToken ct = default);
		public Task<Product> CreateProductAsync(Product model, CancellationToken ct = default);
		public Task<Product> UpdateProductAsync(Product model, CancellationToken ct = default);
		public Task<List<Product>> GetAllProductBySupplierId(int productId, CancellationToken ct = default);
		public Task DeleteAllProductsBySupplierIdAsync(int supplierId, CancellationToken ct = default);

		#endregion


		#region ProductOrder
		public Task DeleteProductOrder(ProductOrder productOrder, CancellationToken ct = default);
		public Task<ProductOrder> CreateProductOrderAsync(ProductOrder model, CancellationToken ct = default);
		#endregion

		public Task SaveChanges(CancellationToken ct = default);
		public Task CreateTransaction(Func<Task> action);
		public Task DeleteAllProductOrdersByOrderIdAsync(int orderId, CancellationToken ct = default);
		public Task<List<ProductOrder>> GetAllProductOrderByOrderIdAsync(int orderId, CancellationToken ct = default);
		public Task<List<ProductOrder>> GetAllProductOrderByProductIdAsync(int productId, CancellationToken ct = default);



	}
}
