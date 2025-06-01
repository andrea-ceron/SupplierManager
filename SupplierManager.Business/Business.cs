using AutoMapper;
using Microsoft.Extensions.Logging;
using SupplierManager.Business.Abstraction;
using SupplierManager.Repository.Abstraction;
using SupplierManager.Repository.Model;
using SupplierManager.Shared.DTO;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SupplierManager.Business
{
	public class Business(IRepository repository, IMapper mapper, ILogger<Business> logger) : IBusiness
	{
		#region Order
		public async Task CreateOrderAsync(CreateOrderDto createOrderDto, CancellationToken ct = default)
		{
			// TODO implementare verifica che permette dio notificare o mandare l errore nel caso in cui l id del prodotto non sia appartenente ad un determinato supplier
			Order order = mapper.Map<Order>(createOrderDto);
			List<ProductOrder> productList = mapper.Map<List<ProductOrder>>(order.ProductOrder);
			order.ProductOrder = new List<ProductOrder>();

			await repository.CreateTransaction(async () =>{

			var newOrder = await repository.CreateOrderAsync(order, ct);
			await repository.SaveChanges(ct);

				logger.LogInformation("Order created with ID: {newOrderId}", newOrder.Id);

				foreach (var product in productList)
				{
					product.OrderId = newOrder.Id;
					await repository.CreateProductOrderAsync(product, ct);
				}
				await repository.SaveChanges(ct);
			});
		}
		public async Task DeleteOrderAsync(int OrderId, CancellationToken ct = default)
		{
			var res = await repository.GetOrderByIdAsync(OrderId, ct);
			if(res == null)
				throw new Exception("Order not found");
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
		public async Task<List<ReadOrderDto>?> GetAllOrdersBySupplierIdAsync(int SupplierId, CancellationToken ct = default)
		{
			List<Order>? ListOfOrder = await repository.GetOrderBySupplierIdAsync(SupplierId, ct);
			if (ListOfOrder == null) throw new Exception("No order found");
			List<ReadOrderDto> orderList = mapper.Map<List<ReadOrderDto>>(ListOfOrder);
			return orderList;
		}
		public async Task<ReadOrderDto> GetOrderByIdAsync(int OrderId, CancellationToken ct = default)
		{
			var Order =  await repository.GetOrderByIdAsync(OrderId, ct);
			if (Order == null ) throw new Exception("Order not found");
			ReadOrderDto orderdto = mapper.Map<ReadOrderDto>(Order);
			return orderdto;
		}
		#endregion

		#region Supplier
		public async Task CreateSupplierAsync(CreateSupplierDto supplierDto, CancellationToken ct = default)
		{
			Supplier supplier = mapper.Map<Supplier>(supplierDto);

			List<Product> productList = mapper.Map<List<Product>>(supplierDto.Products);
			supplier.Products = new List<Product>();


			await repository.CreateTransaction(async() =>
			{

				var supplierRes = await repository.CreateSupplierAsync(supplier, ct);
				await repository.SaveChanges(ct);

				logger.LogInformation("Supplier created with ID: {supplierRes}", supplierRes.Id);

				foreach (var item in productList)
				{
					item.SupplierId = supplierRes.Id;
					await repository.CreateProductAsync(item, ct);
				}
				await repository.SaveChanges(ct);

			});
		}
		public async Task DeleteSupplierAsync(int supplierId, CancellationToken ct = default)
		{
			Supplier ? res = await repository.GetSupplierById(supplierId, ct);
			if (res == null)
				throw new Exception();
			List<Order>? OrderList = await repository.GetOrderBySupplierIdAsync(res.Id, ct);
			if (OrderList == null)
				throw new Exception();
			foreach(var order in OrderList)
			{
				await DeleteOrderAsync(order.Id, ct);
			}
			// eliminazione prodotti
			List<Product>? ProductList = await repository.GetAllProductBySupplierId(res.Id, ct);
			if (ProductList == null)
				throw new Exception();
			await repository.DeleteAllProductsBySupplierIdAsync(ProductList, ct);

			await repository.DeleteSupplier(res, ct);
			await repository.SaveChanges(ct);
		}
		public async Task<ReadSupplierDto> GetSupplierAsync(int supplierId, CancellationToken ct = default)
		{
			
			Supplier? supplier =  await repository.GetSupplierById(supplierId, ct);
			if(supplier == null)
			{
				throw new Exception("da definire");
			}
			ReadSupplierDto supplierDto = mapper.Map<ReadSupplierDto>(supplier);
			return supplierDto;
		}
		public async Task UpdateSupplierAsync(UpdateSupplierDto supplierDto, CancellationToken ct = default)
		{
			var model = mapper.Map<Supplier>(supplierDto);
			var updatedSupplier = await repository.UpdateSupplierAsync(model, ct);
			if(updatedSupplier == null)
				throw new Exception("Supplier not updated");
			await UpdateListOfProductsAsync(supplierDto.Products, ct);
		}
		#endregion

		#region Product
		public async Task CreateListOfProductsAsync(IEnumerable<CreateProductDto> productDto, CancellationToken ct = default)
		{
			List<Product> productList = mapper.Map<List<Product>>(productDto);
			await repository.CreateTransaction(async () =>
			{
				var tasks = productList.Select(product => repository.CreateProductAsync(product, ct));
				await Task.WhenAll(tasks);

				await repository.SaveChanges(ct);
			});

			
		}
		public async Task UpdateListOfProductsAsync(IEnumerable<UpdateProductDto> productDto, CancellationToken ct = default)
		{
			List<Product> productList = mapper.Map<List<Product>>(productDto);
			await repository.CreateTransaction(async () =>
			{
				foreach (var product in productDto)
				{
					if (product.Action == "none") continue;
					if (product.Action == "delete")
					{
						await repository.DeleteProduct(product.Id, ct);
					}
					if (product.Action == "update")
					{
						var model = mapper.Map<Product>(product);
						await repository.UpdateProductAsync(model, ct);
					}
					if (product.Action == "create")
					{
						var model = mapper.Map<Product>(product);
						await repository.CreateProductAsync(model, ct);
					}
					else throw new Exception("Action not defined");

				}
			});
			await repository.SaveChanges(ct);
		}

		public async Task<List<ReadProductDto>> GetProductListBySupplierId(int SupplierId, CancellationToken ct = default)
		{
			var productList = await repository.GetAllProductBySupplierId(SupplierId, ct);
			if (productList == null || productList.Count == 0)
			{
				throw new Exception("No products found for this supplier");
			}
			return mapper.Map<List<ReadProductDto>>(productList);
		}

		public async Task DeleteProductAsync(int productId, CancellationToken ct = default)
		{
			await repository.DeleteProduct(productId, ct);
		}



		#endregion







	}
}
