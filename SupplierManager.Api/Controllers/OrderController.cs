using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SupplierManager.Business.Abstraction;
using SupplierManager.Business;
using SupplierManager.Shared;

namespace SupplierManager.Api.Controllers;


[ApiController]
[Route("[controller]/[action]")]
public class OrderController(IBusiness business, ILogger<SupplierController> logger) : Controller
    {
	private readonly IBusiness _business = business;
	private readonly ILogger<SupplierController> _logger = logger;

	[HttpPost(Name = "CreateOrder")]
	public async Task<ActionResult> CreateOrderAsync(CreateOrderDto payload)
	{
		await _business.CreateOrderAsync(payload.Delivery, payload.SupplierId, payload.ProductOrders);
		return Ok();
	}

	[HttpGet(Name = "ReadOrder")]
	public async Task<ActionResult> GetOrderAsync(int OrderId)
	{
		OrderDto? Order = await _business.GetOrderByIdAsync(OrderId);
		return Ok(Order);
	}

	[HttpGet(Name = "ReadAllOrder")]
	public async Task<ActionResult> GetAllOrderAsync(int OrderId)
	{
		List<OrderDto?>? Order = await _business.GetAllOrdersBySupplierIdAsync(OrderId);
		return Ok(Order);
	}

	[HttpDelete(Name = "DeleteOrder")]
	public async Task<ActionResult> DeleteOrderAsync(int OrderId)
	{
		await _business.DeleteOrderAsync(OrderId);
		return Ok();
	}
}
