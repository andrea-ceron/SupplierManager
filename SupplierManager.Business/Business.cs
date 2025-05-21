using SupplierManager.Business.Abstraction;
using SupplierManager.Repository.Abstraction;
using SupplierManager.Repository.Model;
using SupplierManager.Shared;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SupplierManager.Business
{
	public class Business(IRepository repository) : IBusiness
	{
		public async Task<int> CreateOrderAsync(DateTime delivery, int supplierId, List<ProductOrderDto> products, CancellationToken ct = default)
		{
			var newOrder = await repository.CreateOrderAsync(delivery, supplierId, ct);
			await repository.SaveChanges(ct);
			if (newOrder == null)
				throw new Exception("Order not created");
			foreach (var product in products)
			{
				await repository.CreateProductOrderAsync(product.Quantity, product.Discount, product.ProductId, newOrder.Id, ct);
				await repository.SaveChanges(ct);
			}
			return newOrder.Id;
		}

		public async Task CreateSupplierAsync(SupplierDto supplier, List<ProductDto> productList, CancellationToken ct = default)
		{
			var supplierRes = await repository.CreateSupplierAsync(supplier.CertifiedEmail, supplier.CompanyName, supplier.Email, supplier.Phone, supplier.TaxCode, supplier.VATNumber, ct);
			await repository.SaveChanges(ct);
			if(supplierRes == null)
				throw new Exception("Supplier not created");
			foreach(var product in productList)
			{
				var uploadedProduct = await repository.CreateProductAsync(product.SupplierProductCode, product.Price, product.MinQuantity, supplierRes.Id, ct);
				await repository.SaveChanges(ct);
				if(uploadedProduct == null)
					throw new Exception("Product not created");
			}

		}

		public async Task DeleteOrderAsync(int OrderId, CancellationToken ct = default)
		{
			var res = await repository.GetOrderByIdAsync(OrderId, ct);
			foreach (var productOrder in res.ProductOrder)
			{
				await repository.DeleteProductOrder(productOrder, ct);
				await repository.SaveChanges(ct);
			}
			if (res == null)
				throw new Exception();
			await repository.DeleteOrderAsync(res, ct);
			await repository.SaveChanges(ct);
		}

		public async Task DeleteSupplierAsync(int supplierId, CancellationToken ct = default)
		{
			Supplier ? res = await repository.GetSupplierById(supplierId, ct);
			if (res == null)
				throw new Exception();
			List<Order>? OrderList = await repository.GetOrderBySupplierIdAsync(res.Id, ct);
			if (OrderList == null)
				throw new Exception();
			foreach (var order in OrderList)
			{
				await DeleteOrderAsync(order.Id, ct);
			}
			// eliminazione prodotti
			List<Product>? ProductList = await repository.GetAllProductBySupplierId(res.Id, ct);
			if (ProductList == null)
				throw new Exception();
			foreach (var product in ProductList)
			{
				await repository.DeleteProduct(product, ct);
				await repository.SaveChanges(ct);
			}

			await repository.DeleteSupplier(res, ct);
			await repository.SaveChanges(ct);
		}

		public async Task<List<OrderDto>?> GetAllOrdersBySupplierIdAsync(int SupplierId, CancellationToken ct = default)
		{
			List<Order>? ListOfOrder = await repository.GetOrderBySupplierIdAsync(SupplierId, ct);
			if (ListOfOrder == null) throw new Exception("Supplier not found");

			List<OrderDto> ordersDto = new();
			foreach (var order in ListOfOrder)
			{
				var newOrder = await GetOrderByIdAsync(order.Id, ct);
				if (newOrder == null) throw new Exception("Order not found");
				ordersDto.Add(newOrder);
			}
			return ordersDto;
		}

		public async Task<OrderDto> GetOrderByIdAsync(int OrderId, CancellationToken ct = default)
		{
			var Order =  await repository.GetOrderByIdAsync(OrderId, ct);
			if (Order == null ) throw new Exception("Order not found");
			List<ProductOrderDto> productOrders = new();
			foreach(var productOrder in Order.ProductOrder)
			{
				ProductOrderDto productOrderDto = new()
				{
					Id = productOrder.Id,
					Quantity = productOrder.Quantity,
					Discount = productOrder.Discount,
					ProductId = productOrder.ProductId
				};
				productOrders.Add(productOrderDto);
			}

			OrderDto order = new()
			{
				Id = Order.Id,
				SupplierId = Order.SupplierId,
				DeliveryDate = Order.DeliveryDate,
				ProductOrder = productOrders
			};
			return order;
		}

		public async Task<SupplierDto> GetSupplierAsync(int supplierId, CancellationToken ct = default)
		{
			
			Supplier? res =  await repository.GetSupplierById(supplierId, ct);
			if(res == null)
			{
				throw new Exception("da definire");
			}

			SupplierDto supplier = new()
			{
				Id = res.Id,
				CertifiedEmail = res.CertifiedEmail,
				CompanyName = res.CompanyName,
				Email = res.Email,
				Phone = res.Phone,
				TaxCode = res.TaxCode,
				VATNumber = res.VATNumber,

			};
			return supplier;
		}

		public async Task UpdateSupplierAsync(SupplierDto supplier, List<ProductDto> ProductsToUpdate, CancellationToken ct = default)
		{
			var updatedSupplier = await repository.UpdateSupplierAsync(supplier.Id, supplier.CertifiedEmail, supplier.CompanyName, supplier.Email, supplier.Phone, supplier.TaxCode, supplier.VATNumber, ct);
			if(updatedSupplier == null)
				throw new Exception("Supplier not updated");
			foreach (var product in ProductsToUpdate)
			{
				if (product.Action == "none") continue;
				if(product.Action == "delete")
				{
					var productToDelete = await repository.GetProductById(product.Id, ct);
					await repository.DeleteProduct(productToDelete, ct);
				}
				if (product.Action == "update")
				{
					await repository.UpdateProductAsync(product.Id, product.SupplierProductCode, product.Price, product.MinQuantity, ct);
				}
				if(product.Action == "create")
				{
					await repository.CreateProductAsync(product.SupplierProductCode, product.Price, product.MinQuantity, supplier.Id, ct);
				}
				else throw new Exception("Action not defined");

			}

		}
	}
}
